using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace SouNews.Common {
    public static class DataTableHelper {
        public static DataTable ConvertTo<T>(IList<T> list) {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (T item in list) {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) {
                    row[prop.Name] = prop.GetValue(item);
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows) {
            IList<T> list = null;
            if (rows != null) {
                list = new List<T>();
                foreach (DataRow row in rows) {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }
            return list;
        }

        public static IList<T> ConvertTo<T>(DataTable table) {
            if (table == null) {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows) {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static T CreateItem<T>(DataRow row) {
            T obj = default(T);
            if (row != null) {
                obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in row.Table.Columns) {
                    var columnName = column.ColumnName;
                    //Get property with same columnName
                    PropertyInfo prop = obj.GetType().GetProperty(columnName);
                    try {
                        //Get value for the column
                        object value = (row[columnName] is DBNull)
                            ? null
                            : row[columnName];
                        //Set property value
                        if (prop.CanWrite) //判断其是否可写
                        {
                            prop.SetValue(obj,value,null);
                        }
                    }
                    catch {
                        throw;
                        //Catch whatever here
                    }
                }
            }
            return obj;
        }

        public static DataTable CreateTable<T>() {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties) {
                table.Columns.Add(prop.Name,prop.PropertyType);
            }

            return table;
        }

        public static T GetEntity<T>(DataRow row) where T :new() {
            T entity = new T();
            foreach (var item in entity.GetType().GetProperties()) {
                if (row.Table.Columns.Contains(item.Name)) {
                    if (DBNull.Value != row[item.Name]) {
                        item.SetValue(entity,Convert.ChangeType(row[item.Name],item.PropertyType),null);
                    }
                }
            }

            return entity;
        }

        public static List<T> GetEntitys<T>(DataTable dt) where T :new() {
            List<T> modelList = new List<T>();
            if (dt == null || dt.Rows.Count == 0) {
                return modelList;
            }
            foreach (DataRow dr in dt.Rows) {
                T model = new T();
                foreach (var item in model.GetType().GetProperties()) {
                    if (dr.Table.Columns.Contains(item.Name)) {
                        if (DBNull.Value != dr[item.Name]) {
                            item.SetValue(model,Convert.ChangeType(dr[item.Name],item.PropertyType),null);
                        }
                    }
                }
                modelList.Add(model);
            }
            return modelList;
        }

    }
}