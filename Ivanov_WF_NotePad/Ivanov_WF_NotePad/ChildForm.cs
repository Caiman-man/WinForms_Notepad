using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ivanov_WF_NotePad
{
    //делегат для закрытия формы
    public delegate void CloseMDIDelegate();
    //делегат для пользовательского варианта изменения текста в форме
    public delegate void CustomTextChanged();

    public partial class ChildForm : Form
    {
        public event CloseMDIDelegate PerformCloseMDI;
        public event CustomTextChanged PerformCustomTextChanged;

        public ChildForm(string name)
        {
            InitializeComponent();
            this.Text = name;
            textBox.Text = "";
        }

        //метод для записи текста в форму
        public void setText(string text)
        {
            textBox.Text = text;
        }

        //закрытие формы
        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PerformCloseMDI?.Invoke();
            e.Cancel = false;
        }

        //ивент - был ли изменен текст (внутри которого вызывается пользовательский ивент отслеживающий изменения текста, с доп. параметрами)
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            PerformCustomTextChanged?.Invoke();
            this.textBox.Focus();
        }
    }
}
