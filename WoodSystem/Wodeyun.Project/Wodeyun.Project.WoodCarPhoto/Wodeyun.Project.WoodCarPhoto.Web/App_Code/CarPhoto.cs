using System;

using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.IO;

namespace Wodeyun.Project.WoodCarPhoto.Web.App_Code
{
    /// <summary>
    /// 汽车照片业务处理类
    /// </summary>
    public class CarPhoto
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
        }

        #region 业务

        /// <summary>
        /// 获取照片
        /// </summary>
        /// <param name="unique">照片的文件名称</param>
        /// <returns>照片</returns>
        public System.Drawing.Image GetCarPhoto(int unique)
        {
            StringBuilder sqlBuild = new StringBuilder();
            sqlBuild.Append("select [Photo]");
            sqlBuild.Append(" from [WoodCarPhoto]");
            sqlBuild.Append(" where [Unique] = @Unique");

            SqlParameter[] cmdParameter = {
                new SqlParameter("@Unique", SqlDbType.Int , 4, "Unique")
            };
            cmdParameter[0].Value = unique;

            object sourceImageData = null;
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlBuild.ToString();
                    cmd.Parameters.AddRange(cmdParameter);

                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        while (reader.Read())
                        {
                            sourceImageData = reader[0];
                            break;
                        }
                        cmd.Cancel();
                    }
                    cmd.Parameters.Clear();
                }
            }

            System.Drawing.Image photo = null;
            if (sourceImageData != null)
            {
                Stream st = new MemoryStream((byte[])sourceImageData);
                photo = System.Drawing.Image.FromStream(st, true);
            }

            return photo;
        }

        #endregion
    }
}