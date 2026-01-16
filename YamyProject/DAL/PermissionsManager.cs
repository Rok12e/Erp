using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using YamyProject;

public static class UserPermissions
{
    public static Dictionary<string, bool> ViewPermissions = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    public static Dictionary<string, bool> EditPermissions = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    public static Dictionary<string, bool> DeletePermissions = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

    public static bool canView(string name)
    {
        bool status;
        return ViewPermissions.TryGetValue(name.Trim(), out status) && status;
    }
    public static bool canEdit(string name)
    {
        bool status;
        return EditPermissions.TryGetValue(name.Trim(), out status) && status;
    }
    public static bool canDelete(string name)
    {
        bool status;
        return DeletePermissions.TryGetValue(name.Trim(), out status) && status;
    }
    public static void LoadPermissions(DataTable userTable)
    {
        ViewPermissions.Clear();
        EditPermissions.Clear();
        DeletePermissions.Clear();

        foreach (DataRow row in userTable.Rows)
        {
            string subMenu = row["sub_menu_name"].ToString().Trim();

            bool canView = Convert.ToBoolean(row["can_view"]);
            bool canEdit = Convert.ToBoolean(row["can_edit"]);
            bool canDelete = Convert.ToBoolean(row["can_delete"]);

            ViewPermissions[subMenu] = canView;
            EditPermissions[subMenu] = canEdit;
            DeletePermissions[subMenu] = canDelete;
        }
    }
}

public class PermissionsManager
{
    private readonly HashSet<string> _permissions;

    public PermissionsManager(int roleId)
    {
        _permissions = LoadPermissions(roleId);
    }

    private HashSet<string> LoadPermissions(int roleId)
    {
        var permissions = new HashSet<string>();
        using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                SELECT p.PermissionName
                FROM RolePermissions rp
                JOIN Permissions p ON rp.PermissionId = p.PermissionId
                WHERE rp.RoleId = @RoleId",
                DBClass.CreateParameter("RoleId", roleId)))
            while (reader.Read())
            {
                permissions.Add(reader.GetString("PermissionName"));
            }
        return permissions;
    }

    public bool HasPermission(string permission)
    {
        return _permissions.Contains(permission);
    }
}
