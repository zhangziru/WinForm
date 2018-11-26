using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace 手工实现Xml序列化_反射的应用_
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person() { Name = "Cyrus",Age=25,Email="ClientAndServer@outlook.com"};
            MySerialize(p, p.GetType());
            Console.WriteLine("ok!");
        }
        private static void MySerialize(object obj,Type type)
        {
            //1、判断当前这个类型是否实现了IList<T>接口
            //2、如果实现了该接口则把当前对象当做集合来进行序列化
            //3、如果没有实现该接口，则把当前对象当做一个普通的对象来序列化。
            Type iListType = typeof(IList<Person>);
            if (iListType.IsAssignableFrom(type))
            {
                //按照集合的方式来序列化
            }
            else
            {
                //按照对象的方式来序列化
                //把当前对象写入到xml文件
                //写入xml文件，把类名作为根节点
                XDocument document = new XDocument();
                string nsStr = type.ToString();
                string className = nsStr.Substring(nsStr.LastIndexOf('.')+1);
                //写入一个根节点
                XElement rootElement = new XElement(className);
                //获取当前类型的所有属性
                PropertyInfo[] properties = type.GetProperties();
                //遍历当前类型中的每一个属性
                foreach (PropertyInfo item in properties)
                {
                    //【通过打标记，忽略该属性的序列化】现在该如何获取这个属性是否被标记了MyXmlIgnore标记呢
                    object[] objAttrs = item.GetCustomAttributes(typeof(MyXmlIgnoreAttribute), false);
                    if (objAttrs.Length > 0)
                    {
                        continue;
                    }
                    //将当前的属性写入到xml文件中（其实就是序列化到了该文件中）
                    rootElement.SetElementValue(item.Name,item.GetValue(obj,null));
                }
                document.Add(rootElement);
                document.Save(className + ".xml");
            }
        }
        
    }
    public class Person
    {
        public string Name { get; set; }
        [MyXmlIgnore]
        public int Age { get; set; }
        public string Email { get; set; }
    }
    /// <summary>
    /// 标记，和普通的类没有什么区别，只不过是是继承了Attribute类，打标记时可以省略后缀Attribute
    /// </summary>
    public class MyXmlIgnoreAttribute : Attribute
    {

    }
}
