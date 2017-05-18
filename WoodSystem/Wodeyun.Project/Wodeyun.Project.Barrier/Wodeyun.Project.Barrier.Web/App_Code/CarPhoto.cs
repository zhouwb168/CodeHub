using System;

using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.IO;

namespace Wodeyun.Project.Barrier.Web.App_Code
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
        /// <param name="photoName">照片的文件名称</param>
        /// <returns>照片</returns>
        public System.Drawing.Image GetTempPhoto(string photoName)
        {
            StringBuilder sqlBuild = new StringBuilder();
            sqlBuild.Append("select top 1 [Photo]");
            sqlBuild.Append(" from [WoodTempPhoto]");
            sqlBuild.Append(" where [PhotoName] = @PhotoName");

            SqlParameter[] cmdParameter = {
                new SqlParameter("@PhotoName", SqlDbType.NVarChar , 50, "PhotoName")
            };
            cmdParameter[0].Value = photoName;

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

        /// <summary>
        /// 保存临时照片
        /// </summary>
        /// <param name="photoName">照片的文件名称</param>
        /// <param name="photo">拍摄的照片</param>
        public void SaveTempPhoto(string photoName, byte[] photo)
        {
            StringBuilder sqlBuild = new StringBuilder();
            sqlBuild.Append("insert into [WoodTempPhoto] ([PhotoName], [Photo], [PhotoTime])");
            sqlBuild.Append(" values (@PhotoName, @Photo, getdate())");

            SqlParameter[] cmdParameter = {
                new SqlParameter("@PhotoName", SqlDbType.NVarChar , 50, "PhotoName"),
                new SqlParameter("@Photo", SqlDbType.VarBinary, -1, "Photo")
            };
            cmdParameter[0].Value = photoName;
            cmdParameter[1].Value = photo;

            int affectedRows = -1;
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlBuild.ToString();
                    cmd.Parameters.AddRange(cmdParameter);

                    affectedRows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }

        #endregion
    }
}