using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Musli_MultiTool
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public int TooManyRequests;
        public int MineMusli;
        public int ServerError;
        public int InTotal;
        MusliAPI musliAPI;

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            int musli = await musliAPI.GetMusliAsync();
            double money = await musliAPI.GetMoneyAsync();
            label1.Text = $"Мюсли: {musli}";
            label2.Text = $"Деньги: {money}";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AutoUpdate.Enabled = checkBox1.Checked;
        }

        private async void AutoUpdate_TickAsync(object sender, EventArgs e)
        {
            int musli = await musliAPI.GetMusliAsync();
            double money = await musliAPI.GetMoneyAsync();
            label1.Text = $"Мюсли: {musli}";
            label2.Text = $"Деньги: {money}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") MessageBox.Show("Вы не указали ID пользователя");
            if (textBox3.Text == "") MessageBox.Show("Вы не указали Token Musli");
            if (textBox4.Text == "") MessageBox.Show("Вы не указали vk_ts");
            else
            {
                musliAPI = new MusliAPI(Int32.Parse(textBox1.Text), textBox3.Text, Int32.Parse(textBox4.Text));
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            checkBox1.Checked = false;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private async void AutoClicker_Tick(object sender, EventArgs e)
        {
            int musli = await musliAPI.Click();
            if (musli == 0) TooManyRequests++;
            if (musli == -1) ServerError++;
            if (musli != 0 && musli != -1) MineMusli += musli;
            InTotal++;
            label7.Text = $"Всего запросов: {InTotal}";
            label5.Text = $"Выполнено: {MineMusli}";
            label6.Text = $"Too many requests: {TooManyRequests}";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AutoClicker.Interval = Int32.Parse(textBox2.Text);
            button4.Enabled = false;
            textBox2.Enabled = false;
            button5.Enabled = true;
            AutoClicker.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            textBox2.Enabled = true;
            button5.Enabled = false;
            AutoClicker.Enabled = false;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
