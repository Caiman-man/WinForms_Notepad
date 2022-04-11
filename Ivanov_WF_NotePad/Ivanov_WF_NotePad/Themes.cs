using System;
using System.Windows.Forms;

namespace Ivanov_WF_NotePad
{
    //создаем делегат 
    public delegate void ThemeDelegate(string theme);

    public partial class Themes : Form
    {
        //создаем ивент
        public event ThemeDelegate PerformTheme;

        public Themes()
        {
            InitializeComponent();
        }

        //ивент запускается при нажатии на кнопку 'OK', и в зависимости от выбранной радиокнопки, активируется нужная тема
        private void okButton_Click(object sender, EventArgs e)
        {
            if (lightButton.Checked == true)
                PerformTheme?.Invoke(lightButton.Text);
            else if (darkRaspberryButton.Checked == true)
                PerformTheme?.Invoke(darkRaspberryButton.Text);
            else if (darkBlueButton.Checked == true)
                PerformTheme?.Invoke(darkBlueButton.Text);
            else if (oceanBlueButton.Checked == true)
                PerformTheme?.Invoke(oceanBlueButton.Text);
            else if (darkGreyButton.Checked == true)
                PerformTheme?.Invoke(darkGreyButton.Text);
            else if (limeButton.Checked == true)
                PerformTheme?.Invoke(limeButton.Text);
        }
    }
}
