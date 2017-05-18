using System;
using System.Windows.Forms;

using System.ServiceModel.Description;
using System.Web.UI.WebControls;

using Wodeyun.Bf.Execute.Wrappers;
using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;

namespace Wodeyun.Project.Win
{
    public partial class Logon : Form
    {
        public Logon()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Logon_Load(object sender, EventArgs e)
        {
            ExecutorClient client = new ExecutorClient();
            client.Endpoint.Behaviors.Add(new WebHttpBehavior());
            EntityCollection collection = (EntityCollection)client.Execute("Link", "GetEntities", new object[] { });
            client.Close();

            for (int i = 0; i < collection.Count; i++)
            {
                this.ddlLink.Items.Add(new ListItem(collection[i].GetValue("Name").ToString(), collection[i].GetValue("Unique").ToString()));
            }
            this.ddlLink.SelectedIndex = 0;
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            if (this.txtUsername.Text.Trim() == string.Empty)
            {
                MessageBox.Show("用户名不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtUsername.Focus();
                return;
            }

            if (this.txtPassword.Text.Trim() == string.Empty)
            {
                MessageBox.Show("密码不能为空！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPassword.Focus();
                return;
            }

            ExecutorClient client = new ExecutorClient();
            client.Endpoint.Behaviors.Add(new WebHttpBehavior());
            Entity entity = (Entity)client.Execute("Verificate", "GetEntityByLinkAndUsernameAndPassword", new object[] { ((ListItem)this.ddlLink.SelectedItem).Value.ToInt32(), this.txtUsername.Text, this.txtPassword.Text });
            client.Close();

            if (entity.GetValue("Success").ToBoolean() == false)
            {
                MessageBox.Show(entity.GetValue("Message").ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (entity.GetValue("Value").ToString() == "Id" || entity.GetValue("Value").ToString() == "Link")
                {
                    this.txtUsername.Text = string.Empty;
                    this.txtPassword.Text = string.Empty;
                    this.txtUsername.Focus();
                }
                else
                {
                    this.txtPassword.Text = string.Empty;
                    this.txtPassword.Focus();
                }

                return;
            }

            Entity account = (Entity)((Entity)entity.GetValue("Value")).GetValue("Account");
            Program.Account = account.GetValue("Unique").ToInt32();

            this.DialogResult = DialogResult.OK;
        }
    }
}
