using System;
using System.Collections.Generic;

using System.Collections;

using Wodeyun.Bf.Execute.Wrappers;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.GsmMessage.Helpers
{
    public class MessageHelper : IDisposable
    {
        private PropertyCollection _PropertyCollection = new PropertyCollection();
        private EntityCollection _GsmOrigins;
        private EntityCollection _GsmLines;
        private EntityCollection _GsmSuppliers;
        private EntityCollection _GsmTrees;

        public MessageHelper()
        {
            this._PropertyCollection.Add(new SimpleProperty("Supplier", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Tree", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Make", typeof(Nullable<DateTime>)));
            this._PropertyCollection.Add(new SimpleProperty("Area", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Origin", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("License", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Driver", typeof(string)));
            this._PropertyCollection.Add(new SimpleProperty("Ship", typeof(Nullable<DateTime>)));
            this._PropertyCollection.Add(new SimpleProperty("Line", typeof(string)));

            using (ExecutorClient client = new ExecutorClient())
            {
                this._GsmOrigins = client.Execute("GsmOrigin", "GetEntitiesWithAreaNameByStartAndLength", new object[] { 1, int.MaxValue }) as EntityCollection;
                this._GsmLines = client.Execute("GsmLine", "GetEntities", new object[0]) as EntityCollection;
                this._GsmSuppliers = client.Execute("GsmSupplier", "GetEntities", new object[0]) as EntityCollection;
                this._GsmTrees = client.Execute("GsmTree", "GetEntities", new object[0]) as EntityCollection;
            }
        }

        public EntityCollection GetItemsByText(string text)
        {
            EntityCollection results = new EntityCollection(this._PropertyCollection);

            text = text.Replace(" ", string.Empty);
            IList<string> licenses = this.GetLicenses(text);

            text = this.GetRemain(text, licenses);
            IList<string> drivers = this.GetDrivers(text);

            text = this.GetRemain(text, drivers);
            string supplier = this.GetSupplier(text);

            text = text.Replace(supplier, string.Empty);
            IList<DateTime> times = this.GetTimes(text);

            string tree = this.GetTree(text);
            string origin = this.GetOrigin(text);
            string area = this.GetArea(origin);
            string line = this.GetLine(text);

            for (int i = 0; i < licenses.Count; i++)
            {
                Entity entity = new Entity(this._PropertyCollection);

                entity.SetValue("License", licenses[i]);
                entity.SetValue("Driver", (drivers.Count > i ? drivers[i] : (drivers.Count == 0 ? string.Empty : drivers[drivers.Count - 1])));
                entity.SetValue("Supplier", supplier);
                entity.SetValue("Tree", tree);
                entity.SetValue("Make", (times.Count > 0 ? times[0] as Nullable<DateTime> : null as Nullable<DateTime>));
                entity.SetValue("Area", area);
                entity.SetValue("Origin", origin);
                entity.SetValue("Ship", (times.Count > 1 ? times[1] as Nullable<DateTime> : null as Nullable<DateTime>));
                entity.SetValue("Line", line);

                results.Add(entity);
            }

            return results;
        }

        private IList<string> GetLicenses(string text)
        {
            IList<string> results = new List<string>();

            for (int i = 0; i <= text.Length - "桂A88888".Length; i++)
            {
                string value = text.Substring(i, "桂A88888".Length).ToUpper();

                if (this.IsLicense(value) == true) results.Add(value);
            }

            return results;
        }

        private IList<string> GetDrivers(string text)
        {
            IList<string> results = new List<string>();

            for (int i = 0; i <= text.Length - "13888888888".Length; i++)
            {
                string value = text.Substring(i, "13888888888".Length);

                if (this.IsDriver(value) == true) results.Add(value);
            }

            return results;
        }

        private IList<DateTime> GetTimes(string text)
        {
            IList<DateTime> results = new List<DateTime>();
            IList<int> bads = new List<int>();

            for (int i = 0; i <= text.Length - "00:00".Length; i++)
            {
                string value4 = text.Substring(i, "0:00".Length);
                string value5 = text.Substring(i, "00:00".Length);

                if (this.IsTime(value4) == true && bads.Contains(i) == false)
                {
                    results.Add(("2000-01-01 " + value4.Substring(0, 1) + ":" + value4.Substring(2, 2)).ToDateTime());
                }
                if (this.IsTime(value5) == true)
                {
                    results.Add(("2000-01-01 " + value5.Substring(0, 2) + ":" + value5.Substring(3, 2)).ToDateTime());

                    bads.Add(i + 1);
                }
            }

            return results;
        }

        private string GetSupplier(string text)
        {
            IList<string> name2s = new List<string>();
            IList<string> name3s = new List<string>();

            for (int i = 0; i < this._GsmSuppliers.Count; i++)
            {
                string name = this._GsmSuppliers[i].GetValue("Name").ToString();

                if (name.Length == 2) name2s.Add(name);
                if (name.Length == 3) name3s.Add(name);
            }

            for (int i = 0; i < name3s.Count; i++)
            {
                string name = name3s[i];

                for (int j = 0; j <= text.Length - "A10".Length; j++)
                {
                    string value = text.Substring(j, "A10".Length).ToUpper();

                    if (value == name) return name;
                }
            }

            for (int i = 0; i < name2s.Count; i++)
            {
                string name = name2s[i];

                for (int j = 0; j <= text.Length - "A1".Length; j++)
                {
                    string value = text.Substring(j, "A1".Length).ToUpper();

                    if (value == name) return name;
                }
            }

            return null;
        }

        private string GetTree(string text)
        {
            for (int i = 0; i < this._GsmTrees.Count; i++)
            {
                string name = this._GsmTrees[i].GetValue("Name").ToString();
                string[] aliases = this._GsmTrees[i].GetValue("Alias").ToString().Replace(" ", string.Empty).Split(";,.|_-/\\；，。、—".ToCharArray());

                for (int j = 0; j <= text.Length - name.Length; j++)
                {
                    string value = text.Substring(j, name.Length);

                    if (value == name) return name;
                }

                for (int j = 0; j < aliases.Length; j++)
                {
                    for (int k = 0; k <= text.Length - aliases[j].Length; k++)
                    {
                        string value = text.Substring(k, aliases[j].Length);

                        if (value == aliases[j]) return name;
                    }
                }
            }

            return null;
        }

        private string GetArea(string origin)
        {
            Entity entity = this._GsmOrigins.GetEntity("Name", origin);

            if (entity.IsEmpty == false) return entity.GetValue("Area").ToString();
            else return null;
        }

        private string GetOrigin(string text)
        {
            for (int i = 0; i < this._GsmOrigins.Count; i++)
            {
                string name = this._GsmOrigins[i].GetValue("Name").ToString();
                IList aliases = this._GsmOrigins[i].GetValue("Alias").ToString().Replace(" ", string.Empty).ToList(";,.|_-/\\；，。、—");
                IList excepts = this._GsmOrigins[i].GetValue("Except").ToString().Replace(" ", string.Empty).ToList(";,.|_-/\\；，。、—");

                for (int j = 0; j <= text.Length - name.Length; j++)
                {
                    string value = text.Substring(j, name.Length);

                    if (value == name && this.HasExcepts(text, excepts) == false) return name;
                }

                for (int j = 0; j < aliases.Count; j++)
                {
                    for (int k = 0; k <= text.Length - aliases[j].ToString().Length; k++)
                    {
                        string value = text.Substring(k, aliases[j].ToString().Length);

                        if (value == aliases[j].ToString() && this.HasExcepts(text, excepts) == false) return name;
                    }
                }
            }

            return null;
        }

        private string GetLine(string text)
        {
            for (int i = 0; i < this._GsmLines.Count; i++)
            {
                string name = this._GsmLines[i].GetValue("Name").ToString();
                IList aliases = this._GsmLines[i].GetValue("Alias").ToString().Replace(" ", string.Empty).ToList(";,.|_-/\\；，。、—");

                for (int j = 0; j <= text.Length - name.Length; j++)
                {
                    string value = text.Substring(j, name.Length);

                    if (value == name) return name;
                }

                for (int j = 0; j < aliases.Count; j++)
                {
                    for (int k = 0; k <= text.Length - aliases[j].ToString().Length; k++)
                    {
                        string value = text.Substring(k, aliases[j].ToString().Length);

                        if (value == aliases[j].ToString()) return name;
                    }
                }
            }

            return null;
        }

        private bool IsLicense(string value)
        {
            string first = "京津沪渝蒙新藏宁桂港澳黑吉辽晋冀青鲁豫苏皖浙闽赣湘鄂粤琼甘陕黔滇川";
            string second = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string numbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

            if (first.IndexOf(value.Substring(0, 1)) == -1) return false;
            if (second.IndexOf(value.Substring(1, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(2, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(3, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(4, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(5, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(6, 1)) == -1) return false;

            return true;
        }

        private bool IsDriver(string value)
        {
            string first = "1";
            string numbers = "1234567890";

            if (first.IndexOf(value.Substring(0, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(1, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(2, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(3, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(4, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(5, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(6, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(7, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(8, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(9, 1)) == -1) return false;
            if (numbers.IndexOf(value.Substring(10, 1)) == -1) return false;

            return true;
        }

        private bool IsTime(string value)
        {
            string numbers = "1234567890";
            string hours = "01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23";
            string minutes = "012345";
            string connect = ":.,;：。，；";
            
            if (value.Length == 4)
            {
                if (numbers.IndexOf(value.Substring(0, 1)) == -1) return false;
                if (connect.IndexOf(value.Substring(1, 1)) == -1) return false;
                if (minutes.IndexOf(value.Substring(2, 1)) == -1) return false;
                if (numbers.IndexOf(value.Substring(3, 1)) == -1) return false;
            }

            if (value.Length == 5)
            {
                if (hours.IndexOf(value.Substring(0, 2)) == -1) return false;
                if (connect.IndexOf(value.Substring(2, 1)) == -1) return false;
                if (minutes.IndexOf(value.Substring(3, 1)) == -1) return false;
                if (numbers.IndexOf(value.Substring(4, 1)) == -1) return false;
            }

            return true;
        }

        private bool HasExcepts(string text, IList excepts)
        {
            for (int i = 0; i < excepts.Count; i++)
            {
                for (int j = 0; j <= text.Length - excepts[i].ToString().Length; j++)
                {
                    string value = text.Substring(j, excepts[i].ToString().Length);

                    if (value == excepts[i].ToString()) return true;
                }
            }

            return false;
        }

        private string GetRemain(string text, IList<string> list)
        {
            string result = text;

            for (int i = 0; i < list.Count; i++)
            {
                result = result.Replace(list[i], string.Empty);
            }

            return result;
        }

        public void Dispose()
        {
            return;
        }
    }
}
