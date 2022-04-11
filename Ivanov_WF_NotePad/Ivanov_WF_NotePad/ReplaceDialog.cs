using System;
using System.Windows.Forms;

namespace Ivanov_WF_NotePad
{
    //создаем два делегата для поиска и замены
    public delegate void FindDelegate(string sourceString);
    public delegate void ReplaceDelegate(string sourceString, string newString);

    public partial class ReplaceDialog : Form
    {
        //создаем два ивента
        public event FindDelegate PerformFind;
        public event ReplaceDelegate PerformReplace;

        public ReplaceDialog(string specifier)
        {
            InitializeComponent();
            //выбор вкладки в зависимости от спецификатора
            if (specifier == "find")
                tabControl2.SelectedIndex = 0;
            else if (specifier == "replace")
                tabControl2.SelectedIndex = 1;
            //изначально кнопки поиска и замены неактивны
            findButton.Enabled = false;
            replaceAllButton.Enabled = false;
        }

        //метод, который выбирает какую вкладку необходимо открыть в диалоге, в зависимости от спецификатора
        public void showDialog(string specifier)
        {
            if (specifier == "find")
                tabControl2.SelectedIndex = 0;
            else if (specifier == "replace")
                tabControl2.SelectedIndex = 1;
        }

        //Find
        private void findButton_Click(object sender, EventArgs e)
        {
            PerformFind?.Invoke(textBox1.Text);
        }
       
        //Replace all
        private void replaсeAllButton_Click(object sender, EventArgs e)
        {
            PerformReplace?.Invoke(textBox3.Text, textBox4.Text);
        }
        
        //cancel
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //закрыть(спрятать) форму
        private void ReplaceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        //если текстовые поля для поиска пусты, то кнопка поиска не активна
        private void FindTextBox_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                findButton.Enabled = false;
            if (textBox1.Text.Length > 0)
                findButton.Enabled = true;
        }

        //если текстовые поля для замены пусты, то кнопка замены не активна
        private void ReplaceTextBox_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 0 || textBox4.Text.Length == 0)
                replaceAllButton.Enabled = false;
            if (textBox3.Text.Length > 0 && textBox4.Text.Length > 0)
                replaceAllButton.Enabled = true;
        }

        //ивент, отслеживающий переключение вкладок
        private void tabControl2_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl2.SelectedTab.Text == "Find")
            {
                findButton.Enabled = true;
                replaceAllButton.Enabled = false;
            }
            else if (tabControl2.SelectedTab.Text == "Replace")
            {
                replaceAllButton.Enabled = true;
                findButton.Enabled = false;
            }
        }
    }
}
