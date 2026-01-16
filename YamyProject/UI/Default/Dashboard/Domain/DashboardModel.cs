using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace YamyProject.Domain.Models
{
    public struct RevenueByDate
    {
        public string Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class DashboardModel
    {
        #region Fields & Properties

        private DateTime startDate;
        private DateTime endDate;
        private int numberDays;
        public int NumCustomers { get; private set; }
        public int NumSuppliers { get; private set; }
        public int NumProducts { get; private set; }
        public List<KeyValuePair<string, int>> TopProductsList { get; private set; }
        public List<KeyValuePair<string, int>> UnderstockList { get; private set; }
        public List<RevenueByDate> GrossRevenueList { get; private set; }
        public int NumOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }

        #endregion Fields & Properties

        #region Constructor

        public DashboardModel()
        {
        }

        #endregion Constructor

        #region Private methods

        private void GetNumberItems()
        {
            using (var connection = DBClass.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    //Get Total Number of Customers
                    command.CommandText = "select count(id) from tbl_Customer";
                    NumCustomers = Convert.ToInt32(command.ExecuteScalar());
                    //Get Total Number of Suppliers
                    command.CommandText = "select count(id) from tbl_vendor";
                    NumSuppliers = Convert.ToInt32(command.ExecuteScalar());
                    //Get Total Number of Products
                    command.CommandText = "select count(id) from tbl_items";
                    NumProducts = Convert.ToInt32(command.ExecuteScalar());
                    //Get Total Number of Orders
                    command.CommandText = @"select count(id) from tbl_sales " +
                                            "where date between  @fromDate and @toDate";
                    command.Parameters.Add("@fromDate", MySqlDbType.DateTime).Value = startDate;
                    command.Parameters.Add("@toDate", MySqlDbType.DateTime).Value = endDate;
                    NumOrders = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        private void GetProductAnalisys()
        {
            TopProductsList = new List<KeyValuePair<string, int>>();
            UnderstockList = new List<KeyValuePair<string, int>>();
            using (var connection = DBClass.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    MySqlDataReader reader;
                    command.Connection = connection;
                    //Get Top 5 products
                    command.CommandText = @"select P.name ProductName, sum(tbl_sales_details.qty) as Q
                                            from tbl_sales_details
                                            inner join tbl_items P on P.Id = tbl_sales_details.item_id
                                            inner
                                            join tbl_sales O on O.Id = tbl_sales_details.sales_id
                                            where O.Date between @fromDate and @toDate
                                            group by P.name
                                            order by Q desc LIMIT 5";
                    command.Parameters.Add("@fromDate", MySqlDbType.DateTime).Value = startDate;
                    command.Parameters.Add("@toDate", MySqlDbType.DateTime).Value = endDate;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TopProductsList.Add(
                            new KeyValuePair<string, int>(
                                reader[0].ToString(),
                                Convert.ToInt32(reader[1])
                            )
                        );
                    }
                    reader.Close();
                    //Get Understock
                    command.CommandText = @"select name ProductName,on_hand Stock
                                            from tbl_items
                                            where on_hand <= 6 and state = 0";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UnderstockList.Add(
                            new KeyValuePair<string, int>(reader[0].ToString(), Convert.ToInt32(reader[1])));
                    }
                    reader.Close();
                }
            }
        }
        private void GetOrderAnalisys()
        {
            GrossRevenueList = new List<RevenueByDate>();
            TotalProfit = 0;
            TotalRevenue = 0;
            using (var connection = DBClass.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT 
                                                s.Date AS OrderDate,
                                                SUM(s.total) AS TotalAmount,
                                                SUM(sd.qty * sd.cost_price) AS TotalCost
                                            FROM tbl_sales s
                                            JOIN tbl_sales_details sd ON s.id = sd.sales_id
                                            WHERE s.Date BETWEEN @fromDate AND @toDate
                                            GROUP BY s.Date";
                    command.Parameters.Add("@fromDate", MySqlDbType.DateTime).Value = startDate;
                    command.Parameters.Add("@toDate", MySqlDbType.DateTime).Value = endDate;
                    var reader = command.ExecuteReader();
                    var resultTable = new List<KeyValuePair<DateTime, decimal>>();
                    while (reader.Read())
                    {
                        resultTable.Add(
                            new KeyValuePair<DateTime, decimal>((DateTime)reader[0], (decimal)reader[1])
                            );
                        TotalRevenue += (decimal)reader[1];
                        TotalProfit += (decimal)reader[2];
                    }
                    //TotalProfit = TotalRevenue * 0.2m;//20%
                    reader.Close();
                    //Group by Hours
                    if (numberDays <= 1)
                    {
                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by orderList.Key.ToString("hh")
                                           into order
                                            select new RevenueByDate
                                            {
                                                Date = order.Key,
                                                TotalAmount = order.Sum(amount => amount.Value)
                                            }).ToList();
                    }
                    //Group by Days
                    else if (numberDays <= 30)
                    {
                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by orderList.Key.ToString("dd MMM")
                                           into order
                                            select new RevenueByDate
                                            {
                                                Date = order.Key,
                                                TotalAmount = order.Sum(amount => amount.Value)
                                            }).ToList();
                    }
                    //Group by Weeks
                    else if (numberDays <= 92)
                    {
                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                                orderList.Key, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                           into order
                                            select new RevenueByDate
                                            {
                                                Date = "Week " + order.Key.ToString(),
                                                TotalAmount = order.Sum(amount => amount.Value)
                                            }).ToList();
                    }
                    //Group by Months
                    else if (numberDays <= (365 * 2))
                    {
                        bool isYear = numberDays <= 365 ? true : false;
                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by orderList.Key.ToString("MMM yyyy")
                                           into order
                                            select new RevenueByDate
                                            {
                                                Date = isYear ? order.Key.Substring(0, order.Key.IndexOf(" ")) : order.Key,
                                                TotalAmount = order.Sum(amount => amount.Value)
                                            }).ToList();
                    }
                    //Group by Years
                    else
                    {
                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by orderList.Key.ToString("yyyy")
                                           into order
                                            select new RevenueByDate
                                            {
                                                Date = order.Key,
                                                TotalAmount = order.Sum(amount => amount.Value)
                                            }).ToList();
                    }
                }
            }
        }

        #endregion Private methods

        #region Public methods

        public bool LoadData(DateTime startDate, DateTime endDate)
        {
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day,
                endDate.Hour, endDate.Minute, 59);
            if (startDate != this.startDate || endDate != this.endDate)
            {
                this.startDate = startDate;
                this.endDate = endDate;
                this.numberDays = (endDate - startDate).Days;
                GetNumberItems();
                GetProductAnalisys();
                GetOrderAnalisys();
                Console.WriteLine("Refreshed data: {0} - {1}", startDate.ToString(), endDate.ToString());
                return true;
            }
            else
            {
                Console.WriteLine("Data not refreshed, same query: {0} - {1}", startDate.ToString(), endDate.ToString());
                return false;
            }
        }

        #endregion Public methods
    }
}
