using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XML文件的写入和读取_XDocument_
{
    public partial class Form1 : Form
    {
        Dictionary<string, Person> dict = new Dictionary<string, Person>();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSaveModify_Click(object sender, EventArgs e)
        {
            string id = txtID.Text.Trim();
            string name = txtName.Text.Trim();
            string age = txtAge.Text.Trim();
            string email = txtEmail.Text.Trim();
            //创建一个Person对象
            Person model = new Person() { ID = id, Name = name, Age = int.Parse(age), Email = email };

            //将该对象加入到集合中（因为将来要根据ID找到某个对象，所以Dictionary更方便）
            if (dict.ContainsKey(model.ID))//这里的数据使用，尽量使用我们接受源数据，转换后的数据（不要再使用源数据id变量，因为这样将来出问题，也方便查找）
            {
                //如果集合中已经包含了该ID，则修改 而不是增加
                dict[model.ID] = model;//修改缓存集合中的值
                //修改一下ListBox中的值
                Person currentPerson = lbxInfoView.SelectedItem as Person;
                if (currentPerson != null)
                {
                    lbxInfoView.Items[lbxInfoView.SelectedIndex] = model;
                }
            }
            else
            {
                dict.Add(model.ID, model);
                //将对象增加到集合中后，要让ListBox中显示该数据
                //其实就是向ListBox中也增加一条记录
                lbxInfoView.Items.Add(model);
            }
            ClearTextBox();//添加或修改完毕，清空一下文本框
        }

        /// <summary>
        /// 清空当前窗体 文本框控件
        /// </summary>
        private void ClearTextBox()
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 点击退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();//关闭当前窗体，触发窗体的关闭事件
        }

        private void ExitSave()
        {
            //将dict集合中的内容保存到xml文件中
            //xml操作
            //1、创建一个XDocument对象
            XDocument document = new XDocument();
            //2、加入一个根节点
            XElement rootElement = new XElement("PersonList");
            document.Add(rootElement);
            //3、向根节点中增加Person节点，需要遍历dict集合
            foreach (KeyValuePair<string, Person> item in dict)
            {
                //4、创建一个Person节点
                XElement personElement = new XElement("Person");
                personElement.SetElementValue("ID", item.Value.ID);
                personElement.SetElementValue("Name", item.Value.Name);
                personElement.SetElementValue("Age", item.Value.Age);
                personElement.SetElementValue("Email", item.Value.Email);

                //5、将Person节点添加到根节点
                rootElement.Add(personElement);
            }
            //6、将xml对象写入到文件中
            document.Save("PersonList.xml");
        }
        /// <summary>
        /// 窗体关闭事件（触发xml保存）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitSave();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //如果文件不存在
            if (!File.Exists("PersonList.xml"))
            {
                var tempDoc = new XDocument();//创建xml文档
                tempDoc.Add(new XElement("PersonList"));
                tempDoc.Save("PersonList.xml");
            }
            //将磁盘上的xml文件读取出来 存到dict集合当中 并且 初始化ListBox中的数据
            XDocument document = XDocument.Load("PersonList.xml");
            //获取根节点
            XElement rootElement = document.Root;
            //遍历根节点下的所有子节点
            foreach (XElement item in rootElement.Elements("Person"))
            {
                //创建Person对象               
                string id = item.Element("ID").Value;
                string name = item.Element("Name").Value;
                string age = item.Element("Age").Value;
                string email = item.Element("Email").Value;
                Person model = new Person() { ID = id, Name = name, Age = int.Parse(age), Email = email };
                if (!dict.ContainsKey(model.ID))
                {
                    dict.Add(model.ID, model);
                    //同时也把数据加载到ListBox中
                    lbxInfoView.Items.Add(model);
                }

            }
        }

        /// <summary>
        /// ListBox选择的索引 改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">这个e的类型是最平常的父类。所以放心，这里面没有你想要的东西</param>
        private void lbxInfoView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取当前被选中的项
            if (lbxInfoView.SelectedItem != null)
            {
                Person p = lbxInfoView.SelectedItem as Person;
                if (p != null)
                {
                    txtID.Text = p.ID;
                    txtName.Text = p.Name;
                    txtAge.Text = p.Age.ToString();
                    txtEmail.Text = p.Email;
                }
            }
        }
    }
}
