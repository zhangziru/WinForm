using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm_Home_Test
{
    public partial class UCLogin : UserControl
    {
        public UCLogin()
        {
            InitializeComponent();
        }
        //定义一个用户校验事件
        public event Action UserLoginValidation;
        private void btnOK_Click(object sender, EventArgs e)
        {
            //点击登录按钮，在这里执行校验
            //这里的校验方式不能写死，得把校验方式交给用户来写。
            //所以在这里要定义一个委托，定义一个事件。（我们可以使用系统的委托）
            if (UserLoginValidation!=null)
            {
                UserLoginValidation();
            }
        }
    }

    public class UserLoginEventArgs : EventArgs
    {

    }
}
