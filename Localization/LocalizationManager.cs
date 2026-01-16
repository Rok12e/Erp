using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace YamyProject.Localization
{
    
    public static class LocalizationManager
    {
        private static CultureInfo _uiCulture = new CultureInfo("en"); // default
        private static readonly ResourceManager _resourceManager =
            new ResourceManager("YamyProject.Localization.Localization", typeof(LocalizationManager).Assembly);

        public static void SetLanguage(string cultureCode)
        {
            try
            {
                CultureInfo culture = new CultureInfo(cultureCode);

                if (cultureCode.StartsWith("ar", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Clone and force Gregorian calendar
                    culture = (CultureInfo)culture.Clone();
                    culture.DateTimeFormat.Calendar = new GregorianCalendar();
                    culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                    culture.DateTimeFormat.LongDatePattern = "dddd, dd MMMM yyyy";
                }

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                Properties.Settings.Default.ApplicationLanguage = cultureCode;
                Properties.Settings.Default.Save();

                _uiCulture = culture;
            }
            catch
            {
                var fallbackCulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = fallbackCulture;
                Thread.CurrentThread.CurrentUICulture = fallbackCulture;
                _uiCulture = fallbackCulture;
            }
        }

        public static void ApplySavedLanguage()
        {
            string savedCulture = Properties.Settings.Default.ApplicationLanguage ?? "en";
            SetLanguage(savedCulture);
        }

        public static string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            // Normalize the key: replace spaces with underscores and convert to lowercase
            string normalizedKey = key.Trim().Replace(" ", "_").ToLowerInvariant();

            var nKey = SanitizeResourceKey(normalizedKey);
            // Find the best matching key in resource manager (case-insensitive search)
            var allKeys = _resourceManager
                .GetResourceSet(_uiCulture ?? CultureInfo.CurrentUICulture, true, true)
                .Cast<DictionaryEntry>();

            var match = allKeys.FirstOrDefault(entry =>
                entry.Key is string k && k.ToLowerInvariant() == normalizedKey);

            if (match.Value != null)
            {
                return match.Value.ToString();
            }

            // Fallback: return original string (but replace underscores with spaces)
            return key.Replace("_", " ");
        }

        public static void LocalizeForm(Control container)
        {
            foreach (Control control in container.Controls)
            {
                // Localize control Text
                string localizedText = GetString(control.Text);
                if (!string.IsNullOrEmpty(localizedText))
                    control.Text = localizedText;

                //if(!string.IsNullOrEmpty(localizedText) && localizedText !=control.Text)
                //    Console.WriteLine($"no translation for {control.Text}");

                // Recursive call for child controls
                if (control.HasChildren)
                    LocalizeForm(control);

                // Localize PlaceholderText if it's a Guna2TextBox
                if (control is Guna2TextBox guna2TextBox)
                {
                    string placeholderText = guna2TextBox.PlaceholderText;
                    string localizedPlaceholder = GetString(placeholderText);
                    if (!string.IsNullOrEmpty(localizedPlaceholder))
                        guna2TextBox.PlaceholderText = localizedPlaceholder;

                    //if (!string.IsNullOrEmpty(localizedPlaceholder) && localizedPlaceholder != guna2TextBox.PlaceholderText)
                    //    Console.WriteLine($"no translation for {placeholderText}");
                }

                // Localize DataGridView columns
                LocalizeDataGridViewColumns(control);

                // Handle Right-to-Left layout
                if (Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
                {
                    control.RightToLeft = RightToLeft.Yes;
                    if (control is Form form)
                        form.RightToLeftLayout = true;
                }

                // Now handle ToolStrip controls attached to this Control (like in Form or Panel)
                if (control is ToolStrip toolStrip)
                {
                    LocalizeToolStripItems(toolStrip.Items);
                }
            }
        }

        // Helper method to localize columns of various DataGridViews
        private static void LocalizeDataGridViewColumns(Control control)
        {
            if (control is Guna2DataGridView gunaDgv)
            {
                foreach (DataGridViewColumn column in gunaDgv.Columns)
                    LocalizeColumnHeader(column);
            }
            else if (control is Guna2DataGridView guna2Dgv)
            {
                foreach (DataGridViewColumn column in guna2Dgv.Columns)
                    LocalizeColumnHeader(column);
            }
            else if (control is DataGridView dgv)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                    LocalizeColumnHeader(column);
            }
        }

        private static void LocalizeColumnHeader(DataGridViewColumn column)
        {
            if (!string.IsNullOrWhiteSpace(column.HeaderText))
            {
                string localizedHeader = GetString(column.HeaderText);
                if (!string.IsNullOrEmpty(localizedHeader) && localizedHeader != column.HeaderText)
                    column.HeaderText = localizedHeader;

                //if (!string.IsNullOrEmpty(localizedHeader) && localizedHeader != column.HeaderText)
                //    Console.WriteLine($"no translation for {column.HeaderText}");
            }
        }

        // Recursive method to localize ToolStripMenuItems and other ToolStripItems
        private static void LocalizeToolStripItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (!string.IsNullOrWhiteSpace(item.Text))
                {
                    string localizedText = GetString(item.Text);
                    if (!string.IsNullOrEmpty(localizedText) && localizedText != item.Text)
                        item.Text = localizedText;
                    //else
                    //    Console.WriteLine($"no translation for {item.Text}");
                }

                // ToolStripMenuItems may have dropdown items (submenus)
                if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                {
                    LocalizeToolStripItems(menuItem.DropDownItems);
                }
            }
        }

        public static void LocalizeFormOnControllerOnly(Control container)
        {
            foreach (Control control in container.Controls)
            {
                bool isTranslatableType = control is Label || control is Guna2HtmlLabel || control is Guna2Button || control is Button || control is TextBox
                                        || control is CheckBox || control is RadioButton;
                if(control.Name == "label7")
                {
                    string t = control.Text;
                }
                if (isTranslatableType && !string.IsNullOrWhiteSpace(control.Text))
                {
                    string originalText = control.Text; 
                    string originalName = control.Name;
                    string localizedText = GetString(originalText);

                    if (!string.IsNullOrEmpty(localizedText) && localizedText != originalText)
                    {
                        control.Text = localizedText;
                        //Console.WriteLine($"no translation for {control.Text}");
                    }
                    else
                    {
                        control.Text = originalText;
                    }
                    if (_uiCulture.TextInfo.IsRightToLeft)
                    {
                        if (control is Label || control is RadioButton)
                            control.RightToLeft = RightToLeft.Yes;
                        else
                            control.RightToLeft = RightToLeft.No;
                    }

                } 
                else if (isTranslatableType)
                {
                    control.Text = control.Text;
                }

                // Apply RightToLeft for RTL languages
                if (Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
                {
                    control.RightToLeft = RightToLeft.Yes;

                    if (control is Form form)
                        form.RightToLeftLayout = true;
                }

                // Recursively apply to children
                if (control.HasChildren)
                    LocalizeForm(control);
            }
        }

        public static void LocalizeDataGridViewHeaders(DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                string localized = GetString(column.HeaderText);
                if (!string.IsNullOrEmpty(localized) && localized != column.HeaderText)
                    column.HeaderText = localized;

                //if (!string.IsNullOrEmpty(localized) && localized != column.HeaderText)
                //    Console.WriteLine($"no translation for {column.HeaderText}");
            }
        }

        public static string SanitizeResourceKey(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Replace spaces and invalid chars with underscore
            var invalidChars = new HashSet<char>(System.IO.Path.GetInvalidFileNameChars());
            invalidChars.UnionWith(new char[] { ' ', '-', '.', ',', '\\', '/', ':', ';', '|', '*', '?', '"', '\'', '<', '>', '&', '%', '$', '#', '@', '!', '^', '(', ')', '[', ']', '{', '}', '=', '+', '~', '`', '▼' });

            var sb = new System.Text.StringBuilder();
            foreach (var ch in input)
            {
                if (char.IsLetterOrDigit(ch) || ch == '_')
                {
                    sb.Append(ch);
                }
                else
                {
                    // Replace invalid char with underscore
                    sb.Append('_');
                }
            }

            // Optional: remove repeated underscores
            string result = System.Text.RegularExpressions.Regex.Replace(sb.ToString(), "_+", "_");

            // Optional: trim underscores at start/end
            result = result.Trim('_');

            // If empty, return default key
            if (string.IsNullOrEmpty(result))
                return "";

            return result;
        }
    }
}
