using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace XML作为数据库_增删改查_
{
    public partial class Form1 : Form
    {

        //缺点：每次都要加载全部xml的内容
        //为什么，不把XmlDocument 的对象放在外面，只创建一次？
        //如果把文档对象作为类的成员变量，那么什么时候把内容写进去。比如：修改完文件以后，我立即写入到了文件里面。
        //如果作为成员变量，那么修改文件内容，也是在程序的内容中进行修改的，在程序退出或保存时候在写入。在运行期间如果xml文件有人改了怎么办
        //所以，不作为类成员，可以保证，每次的数据都是新的。
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            listView1.Items.Clear();
            #region 窗体加载的时候把数据显示到ListView中（ListView属性View选择Detail）
            //1.读取XML文件 
            XmlDocument document = new XmlDocument();
            document.Load("UserData.xml");
            XmlNodeList nodeList = document.SelectNodes("/Users/user");
            //遍历选择到的节点，加载到ListView中
            foreach (XmlNode userNode in nodeList)
            {
                //创建一个ListViewItem中的一个项
                ListViewItem lvItem = new ListViewItem(userNode.Attributes["id"].Value);
                //获取name节点并绑定
                lvItem.SubItems.Add(userNode.SelectSingleNode("name").InnerText);
                lvItem.SubItems.Add(userNode.SelectSingleNode("password").InnerText);
                listView1.Items.Add(lvItem);
            }
            #endregion
        }
        //增加某个节点
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string id = txtId.Text.Trim();
            string name = txtLoginId.Text.Trim();
            string password = txtPwd.Text.Trim();
            XmlDocument document = new XmlDocument();
            document.Load("UserData.xml");
            XmlElement root = document.DocumentElement;
            XmlElement userElement = document.CreateElement("user");
            if (document.SelectNodes("/Users/user[@id='" + id + "']").Count > 0)
            {
                MessageBox.Show("id重复！");
            }
            else
            {
                userElement.SetAttribute("id", id);
                XmlElement nameElement = document.CreateElement("name");
                nameElement.InnerText = name;

                XmlElement passwordElement = document.CreateElement("password");
                passwordElement.InnerText = password;

                userElement.AppendChild(nameElement);
                userElement.AppendChild(passwordElement);

                root.AppendChild(userElement);
                document.Save("UserData.xml");
                LoadUsers();
                MessageBox.Show("ok!");
            }
        }
        //删除某个节点
        private void btnDelete_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load("UserData.xml");
            if (listView1.SelectedItems.Count > 0)
            {
                //获取选中行的id
                string id = listView1.SelectedItems[0].SubItems[0].Text;

                //从xml中找到id等于用户勾选的id的user标签
                XmlNode node = document.SelectSingleNode("/Users/user[@id='"+ id+"']");

                if (node != null)
                {
                    //从文档的根节点开始，删除当前选中的节点
                    document.DocumentElement.RemoveChild(node);
                    document.Save("UserData.xml");
                    LoadUsers();
                }
            }
        }
        //选中ListView中的项，将值动态映射到编辑区
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem lv = listView1.SelectedItems[0];
                txtEditId.Text = lv.SubItems[0].Text;
                txtEditLoginId.Text = lv.SubItems[1].Text;
                txtEditPwd.Text = lv.SubItems[2].Text;
            }
        }
        /// <summary>
        /// 修改某个节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load("UserData.xml");
            //根据用户选择的id，现在UserData.xml中找到对应的user节点
            XmlNode userNode = document.SelectSingleNode("/Users/user[@id='" + txtEditId.Text.Trim() + "']");
            userNode.SelectSingleNode("name").InnerText = txtEditLoginId.Text.Trim();
            userNode.SelectSingleNode("password").InnerText = txtEditPwd.Text.Trim();

            document.Save("UserData.xml");
            LoadUsers();
        }

        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string uid = txtUserId.Text.Trim();
            string pwd = txtUserPwd.Text.Trim();

            XmlDocument document = new XmlDocument();
            document.Load("UserData.xml");

            XmlNode node = document.SelectSingleNode("/Users/user/name[.='"+uid+"']");
            if (node != null)
            {
                if (node.NextSibling.InnerText == pwd)
                {
                    MessageBox.Show("登录成功！");
                }
                else
                {
                    MessageBox.Show("密码错误！");
                }
            }
            else
            {
                MessageBox.Show("用户名不存在！");
            }
        }
    }
}
