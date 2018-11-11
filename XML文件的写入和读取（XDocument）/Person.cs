using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML文件的写入和读取_XDocument_
{
    public class Person
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 重写对象ToString方法（对象默认的该方法，返回的是：命名空间+类名）
        /// </summary>
        /// <returns>对象姓名</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// 对象实例化 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="email"></param>
        public Person(string id, string name, int age, string email)
        {
            this.ID = id;
            this.Name = name;
            this.Age = age;
            this.Email = email;
        }
    }
}
