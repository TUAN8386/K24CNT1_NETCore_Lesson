using System;
using System.Collections.Generic;
using System.Linq;

namespace DAT_LAB_GUIDE_04.Models
{
    public class DAT_DataLocal
    {
        public static List<DAT_People> peoples = new List<DAT_People>()
        {
            new DAT_People() {
                Id = 0, Name = "DH NG TRÃI", Email = "dhnt.edu.vn@gmail.com",
                Phone = "0976611112", Address = "25 Phan Xích Long",
                Avatar = "images/avatar/1.png", Birthday = Convert.ToDateTime("2012/09/22"),
                Bio = "TRƯỜNG ĐẠI HỌC NGUYỄN TRÃI", Gender = 0
            },
            new DAT_People() {
                Id = 1, Name = "ĐINH ANH TUẤN", Email = "tuandinh@gmail.com",
                Phone = "0978611112", Address = "25 Phan đình giót",
                Avatar = "images/avatar/1.jpg", Birthday = Convert.ToDateTime("2006/05/09"),
                Bio = "Master", Gender = 1
            },
            new DAT_People() {
                Id = 2, Name = "Nguyễn Huy", Email = "huynguyen@gmail.com",
                Phone = "0922113113", Address = "Gia Lâm, Hà Nội",
                Avatar = "images/avatar/2.jpg", Birthday = Convert.ToDateTime("1999/02/12"),
                Bio = "Master", Gender = 1
            }
        };

        public static List<DAT_People> GetPeoples()
        {
            return peoples;
        }

        public static DAT_People? GetPeopleById(int id)
        {
            return peoples.FirstOrDefault(x => x.Id == id);
        }
    }
}