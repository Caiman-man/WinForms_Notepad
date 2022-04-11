using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Ivanov_WF_NotePad
{
    public partial class Form1 : Form
    {
        //список имен (сколько вкладок столько и имен)
        List<string> names = new List<string>();

        //список bool для каждого документа (был ли он изменен)
        List<bool> changes = new List<bool>();
        
        //список bool для каждого документа (был ли он сохранен)
        List<bool> saves = new List<bool>();

        //кол-во удаленных вкладок
        int del = 0;

        public Form1()
        {
            //поскольку форма стартует с уже созданным документом, то ему сразу нужно присвоить пустое имя
            //и обозначить, что документ не был изменен и не был сохранен
            names.Add("");
            changes.Add(false);
            saves.Add(false);
            InitializeComponent();
            cascadeMDIToolStripMenuItem.Enabled = false;
            horisontalMDIToolStripMenuItem.Enabled = false;
            verticalMDIToolStripMenuItem.Enabled = false;
        }

        //new
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //вкладка создается независимо от режима
            names.Add("");
            changes.Add(false);
            saves.Add(false);
            //считаем кол-во не сохраненных документов, что бы создавать новый документ с порядковым номером ("New 1, New 2...")
            int num = 0;
            for (int i = 0; i < saves.Count; i++)
            {
                if (saves[i] == false)
                    num++;
            }
            //два цикла для проверки нумерации новых вкладок - работает неправильно
            //иногда при удалении вкладки и при создании новой, создается вкладка с удаленным индексом, а иногда с новым последующим индексом
            //но одинаковых вкладок не бывает
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                if (tabControl1.TabPages[i].Text == $"New {num}")
                    num++;
            }
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                if (tabControl1.TabPages[i].Text == $"New {num}")
                    num++;
            }
            //создаем вкладку с документом
            TabPage page = new TabPage($"New {num}");
            TextBox textBox = new TextBox();
            textBox.Text = "";
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox.Font = new Font("Calibri", 14);
            textBox.Parent = page;
            textBox.TextChanged += textBox_TextChanged;
            tabControl1.TabPages.Add(page);
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
            pictureBox1.Visible = true;
            CurrentTheme();
            //если включен mdi-режим, то создаем еще и окно
            if (isMDI == true)
            {
                pictureBox1.Visible = false;               
                documents.Add(tabControl1.SelectedTab.Controls[0].Text);
                ChildForm f = new ChildForm(tabControl1.SelectedTab.Text);
                f.MdiParent = this;
                f.Controls[0].BackColor = tabControl1.SelectedTab.Controls[0].BackColor;
                f.Controls[0].Font = tabControl1.SelectedTab.Controls[0].Font;
                f.Controls[0].ForeColor = tabControl1.SelectedTab.Controls[0].ForeColor;
                f.PerformCloseMDI += CloseMDI;
                f.setText(documents[tabControl1.SelectedIndex]);
                f.PerformCustomTextChanged += CustomTextChanged;
                f.Show();
                forms.Add(f);
            }
        }

        //open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //документ открывается в новой вкладке независимо от режима
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                //запрещаем открывать файл, если такой уже открыт в списке вкладок
                if (names.Contains(fileName))
                {
                    MessageBox.Show("The file is already opened!");
                    return;
                }
                //добавляем имя и признаки изменения и сохранения
                names.Add(fileName);
                changes.Add(false);
                saves.Add(true);
                try
                {
                    TabPage page = new TabPage(Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName));
                    TextBox textBox = new TextBox();
                    textBox.Text = "";
                    textBox.Dock = DockStyle.Fill;
                    textBox.Multiline = true;
                    textBox.ScrollBars = ScrollBars.Vertical;
                    textBox.Font = new Font("Calibri", 14);
                    textBox.BackColor = Color.White;
                    textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    textBox.Parent = page;
                    textBox.TextChanged += textBox_TextChanged;
                    tabControl1.TabPages.Add(page);
                    tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                    pictureBox1.Visible = true;
                    CurrentTheme();
                    //читаем файл
                    textBox.Text = File.ReadAllText(fileName);  
                    //поскольку после прочтения файла и записи текста в textbox документ будет считаться измененным, то в его названии появится звездочка
                    //поэтому удаляем звездочку в названии и ставим признак, того что документ не был изменен
                    if (changes[tabControl1.SelectedIndex] == true)
                    {
                        tabControl1.SelectedTab.Text = tabControl1.SelectedTab.Text.Remove(0, 1);
                        changes[tabControl1.SelectedIndex] = false;
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to open file!");
                }
            }
            //если включен mdi-режим, то создаем еще и окно
            if (isMDI == true)
            {
                pictureBox1.Visible = false;
                documents.Add(tabControl1.SelectedTab.Controls[0].Text);
                ChildForm f = new ChildForm(tabControl1.SelectedTab.Text);
                f.MdiParent = this;
                f.Controls[0].BackColor = tabControl1.SelectedTab.Controls[0].BackColor;
                f.Controls[0].Font = tabControl1.SelectedTab.Controls[0].Font;
                f.Controls[0].ForeColor = tabControl1.SelectedTab.Controls[0].ForeColor;
                f.PerformCloseMDI += CloseMDI;
                f.setText(documents[tabControl1.SelectedIndex]);
                f.PerformCustomTextChanged += CustomTextChanged;
                f.Show();
                forms.Add(f);
            }
        }

        //метод сохранения файлов (данные для сохранения всегда берутся из вкладок)
        public void Save(string newFileName)
        {
            saveFileDialog1.Title = $"Save {tabControl1.SelectedTab.Text}";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            //если у файла нет имени, значит файл - новый, значит обязательно вызываем диалог сохранения
            if (newFileName == "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    newFileName = saveFileDialog1.FileName;
                    //запрещаем сохранять файл, если такой уже открыт в списке вкладок
                    if (names.Contains(newFileName))
                    {
                        MessageBox.Show("The file is already opened!");
                        return;
                    }
                    //меняем имя файла в списке names
                    names[tabControl1.SelectedIndex] = newFileName;
                    //меняем имя в названии вкладки
                    tabControl1.SelectedTab.Text = Path.GetFileNameWithoutExtension(newFileName) + Path.GetExtension(newFileName);
                }
                else
                    return;
            }
            try
            {
                if (tabControl1.SelectedTab.Controls[0] is TextBox)
                    File.WriteAllText(newFileName, tabControl1.SelectedTab.Controls[0].Text);
                //убираем звездочку в имени документа
                if (tabControl1.SelectedTab.Text[0] == '*')
                    tabControl1.SelectedTab.Text = tabControl1.SelectedTab.Text.TrimStart('*');
                //обозначаем, что файл перестал быть измененным
                changes[tabControl1.SelectedIndex] = false;
                //обозначаем, что файл был сохранен
                saves[tabControl1.SelectedIndex] = true;
            }
            catch
            {
                MessageBox.Show("Unable to save file!");
            }
        }

        //save
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (isMDI == false)
                Save(names[tabControl1.SelectedIndex]);
            else
            {
                //делаем активной ту вкладку, которая совпадает по индексу с активным mdi-окном
                if (forms.IndexOf((ChildForm)this.ActiveMdiChild) >= 0)
                    tabControl1.SelectedIndex = forms.IndexOf((ChildForm)this.ActiveMdiChild);
                //копируем текст во вкладку и сохраняем её
                tabControl1.SelectedTab.Controls[0].Text = ((ChildForm)this.ActiveMdiChild).Controls[0].Text;
                Save(names[tabControl1.SelectedIndex]);
                ((ChildForm)this.ActiveMdiChild).Text = tabControl1.SelectedTab.Text;
            }
        }

        //save as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isMDI == false)
                Save("");   //в данном методе мы специально передаем пустое имя, что бы вызвать диалог сохранения
            else
            {
                //делаем активной ту вкладку, которая совпадает по индексу с активным mdi-окном
                if (forms.IndexOf((ChildForm)this.ActiveMdiChild) >= 0)
                    tabControl1.SelectedIndex = forms.IndexOf((ChildForm)this.ActiveMdiChild);
                //копируем текст во вкладку и сохраняем её
                tabControl1.SelectedTab.Controls[0].Text = ((ChildForm)this.ActiveMdiChild).Controls[0].Text;
                Save("");
                ((ChildForm)this.ActiveMdiChild).Text = tabControl1.SelectedTab.Text;
            }
        }

        //save all
        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isMDI == false)
            {
                if (tabControl1.TabCount != 0)
                {
                    for (int i = 0; i < tabControl1.TabCount; i++)
                    {
                        tabControl1.SelectedIndex = i;
                        Save(names[i]);
                    }
                }
            }               
            else
            {
                if (tabControl1.TabCount != 0)
                {
                    for (int i = 0; i < tabControl1.TabCount; i++)
                    {
                        tabControl1.SelectedIndex = i;
                        tabControl1.SelectedTab.Controls[0].Text = this.MdiChildren[i].Controls[0].Text;
                        Save(names[i]);
                        this.MdiChildren[i].Text = tabControl1.SelectedTab.Text;
                    }
                }
            }
            
        }     

        //close | нажатие на картинку с крестиком
        private void CloseDocument_Click(object sender, EventArgs e)
        {
            if (isMDI == false)
            {
                //если вкладок нет, то кнопка блокируется
                if (tabControl1.TabCount == 0)
                    pictureBox1.Enabled = false;
                else
                {
                    QuestionSaveFile();
                    //удаляем документ из списка names, changes и saves
                    names.RemoveAt(tabControl1.SelectedIndex);
                    changes.RemoveAt(tabControl1.SelectedIndex);
                    saves.RemoveAt(tabControl1.SelectedIndex);
                    tabControl1.Controls.RemoveAt(tabControl1.SelectedIndex);
                    del++;
                    //обнуляем кол-во символов в статус-баре
                    toolStripStatusLabel1.Text = "";
                    //если вкладок нет, то кнопка с крестиком - становится невидима
                    if (tabControl1.TabCount == 0)
                        pictureBox1.Visible = false;
                }
                //если все вкладки закрыты, то создаем новую - отключено
                //if (tabControl1.TabCount == 0)
                //newToolStripMenuItem_Click(sender, e);
            }
            else
                ((ChildForm)this.ActiveMdiChild).Close();
        }

        //close all
        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 0)
            {
                int n = tabControl1.TabPages.Count;
                for (int i = 0; i < n; i++)
                    CloseDocument_Click(sender, e);
            }
        }

        //exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //закрытие окна
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //сначала закрываем все вкладки
            closeAllToolStripMenuItem_Click(sender, e);
            e.Cancel = false;
        }

        //QuestionSaveFile - метод, если в документе были изменения, то предложить пользователю сохранить документ
        public void QuestionSaveFile()
        {
            if (changes[tabControl1.SelectedIndex] == true)
            {
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show($"Save changes in {tabControl1.SelectedTab.Text.TrimStart('*')}?", "Saving file", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                    Save(names[tabControl1.SelectedIndex]);
            }
        }

        //обработчик текстового поля (если текст был изменен)
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            //добавялем признак, того что документ был изменен
            changes[tabControl1.SelectedIndex] = true;
            //добавялем звездочку в название документа (во вкладке)
            if (!tabControl1.SelectedTab.Text.Contains("*"))
                tabControl1.SelectedTab.Text = tabControl1.SelectedTab.Text.Insert(0, "*");
            //пишем кол-во символов в статус-баре
            TextBox main = tabControl1.SelectedTab.Controls[0] as TextBox;
            toolStripStatusLabel1.Text = "Number of words: " + (main.SelectionStart + main.SelectionLength).ToString();
        }

        //about
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Owner = this;
            DialogResult res = about.ShowDialog();
        }

        //print
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //объект для печати
            PrintDocument printDocument = new PrintDocument();
            //обработчик события печати
            printDocument.PrintPage += PrintPageHandler;
            //диалог настройки печати
            PrintDialog printDialog = new PrintDialog();
            //установка объекта печати для его настройки
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
                printDialog.Document.Print();
        }

        //обработчик события печати
        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            // печать строки result
            if (isMDI == false)
                e.Graphics.DrawString(textBox1.Text, new Font("Calibri", 14), Brushes.Black, 0, 0);
            else
            {
                tabControl1.SelectedTab.Controls[0].Text = ((ChildForm)this.ActiveMdiChild).Controls[0].Text;
                e.Graphics.DrawString(textBox1.Text, new Font("Calibri", 14), Brushes.Black, 0, 0);
            }
        }

        //-----------------------------------------------------------------------------------------
        //FIND / REPLACE

        //создаем диалог
        ReplaceDialog dialog = null;

        //вызов окна поиска
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //создаем окно поиска/замены и добавляем соответствующие ивенты
            if (dialog == null)
            {
                dialog = new ReplaceDialog("find");
                dialog.PerformFind += FindDialog_PerformFind;
                dialog.PerformReplace += ReplaceDialog_PerformReplace;
            }
            if (tabControl1.TabPages.Count != 0) //если вкладок нет, то диалог не запустится
            {
                dialog.Show();
                dialog.Focus();
                dialog.showDialog("find");      //сразу открывается вкладка 'find'
            }
        }

        //вызов окна замены
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //создаем окно поиска/замены и добавляем соответствующие ивенты
            if (dialog == null)
            {
                dialog = new ReplaceDialog("replace");
                dialog.PerformFind += FindDialog_PerformFind;
                dialog.PerformReplace += ReplaceDialog_PerformReplace;
            }
            if (tabControl1.TabPages.Count != 0) //если вкладок нет, то диалог не запустится
            {
                dialog.Show();
                dialog.Focus();
                dialog.showDialog("replace");   //сразу открывается вкладка 'replace all'
            }
        }

        //ивент, который запускается по нажатию на кнопку find
        int pos = 0;
        private void FindDialog_PerformFind(string sourceStr)
        {
            //для tab-режима
            if (isMDI == false)
            {
                if (tabControl1.SelectedTab.Controls[0] is TextBox)
                {
                    TextBox main = tabControl1.SelectedTab.Controls[0] as TextBox;
                    if (main.Text.Contains(sourceStr))
                    {
                        pos = main.Text.IndexOf(sourceStr, pos);
                        if (pos != -1)
                        {
                            main.SelectionStart = pos;
                            main.SelectionLength = sourceStr.Length;
                            pos += sourceStr.Length;
                        }
                        else
                        {
                            MessageBox.Show("No more occurrences found in the specified document");
                            pos = 0;
                        }
                    }
                    else
                        MessageBox.Show($"{sourceStr} is not found!");
                    main.Focus();
                }
            }
            else    //для mdi-режима
            {
                if (((ChildForm)this.ActiveMdiChild).Controls[0] is TextBox)
                {
                    TextBox main = ((ChildForm)this.ActiveMdiChild).Controls[0] as TextBox;
                    if (main.Text.Contains(sourceStr))
                    {
                        pos = main.Text.IndexOf(sourceStr, pos);
                        if (pos != -1)
                        {
                            main.SelectionStart = pos;
                            main.SelectionLength = sourceStr.Length;
                            pos += sourceStr.Length;
                        }
                        else
                        {
                            MessageBox.Show("No more occurrences found in the specified document");
                            pos = 0;
                        }
                    }
                    else
                        MessageBox.Show($"{sourceStr} is not found!");
                    main.Focus();
                }
            }           
        }

        //ивент, который запускается по нажатию на кнопку replace all
        private void ReplaceDialog_PerformReplace(string sourceStr, string newStr)
        {
            //для tab-режима
            if (isMDI == false)
            {
                if (tabControl1.SelectedTab.Controls[0] is TextBox)
                {
                    TextBox main = tabControl1.SelectedTab.Controls[0] as TextBox;
                    string doc = main.Text;
                    string newDoc = doc.Replace(sourceStr, newStr);
                    main.Text = newDoc;
                }
            }
            else    //для mdi-режима
            {
                if (((ChildForm)this.ActiveMdiChild).Controls[0] is TextBox)
                {
                    TextBox main = ((ChildForm)this.ActiveMdiChild).Controls[0] as TextBox;
                    string doc = main.Text;
                    string newDoc = doc.Replace(sourceStr, newStr);
                    main.Text = newDoc;
                }
            }
        }

        //-----------------------------------------------------------------------------------------
        //OPTIONS

        //выбор шрифта
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount != 0)
            {
                fontDialog1.ShowColor = true;
                fontDialog1.Color = Color.Black;
                fontDialog1.MinSize = 20;
                fontDialog1.MaxSize = 50;
                if (fontDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (isMDI == false)
                    {
                        tabControl1.SelectedTab.Controls[0].Font = fontDialog1.Font;
                        tabControl1.SelectedTab.Controls[0].ForeColor = fontDialog1.Color;
                    }
                    else
                    {
                        if (forms.IndexOf((ChildForm)this.ActiveMdiChild) >= 0)
                            tabControl1.SelectedIndex = forms.IndexOf((ChildForm)this.ActiveMdiChild);
                        this.MdiChildren[tabControl1.SelectedIndex].Controls[0].Font = fontDialog1.Font;
                        this.MdiChildren[tabControl1.SelectedIndex].Controls[0].ForeColor = fontDialog1.Color;
                    }
                }
            }
        }

        //выбор цвета страницы
        private void pageColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //если вкладок нет, то выбираем цвет страницы
            if (tabControl1.TabCount != 0)
            {
                colorDialog1.FullOpen = true;
                colorDialog1.AllowFullOpen = true;
                DialogResult result = colorDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (isMDI == false)
                        tabControl1.SelectedTab.Controls[0].BackColor = colorDialog1.Color;
                    else
                    {
                        if (forms.IndexOf((ChildForm)this.ActiveMdiChild) >= 0)
                            tabControl1.SelectedIndex = forms.IndexOf((ChildForm)this.ActiveMdiChild);
                        this.MdiChildren[tabControl1.SelectedIndex].Controls[0].BackColor = colorDialog1.Color;
                    }
                }
            }
        }

        //вызов окна - themes
        private void themesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Themes theme = new Themes();
            theme.Owner = this;
            theme.PerformTheme += ThemeDialog_ChangeTheme;
            DialogResult res = theme.ShowDialog();
        }

        //название текущей темы
        string currentTheme = "";

        //ивент - выбор темы (запускается при нажатии на кнопку 'OK')
        private void ThemeDialog_ChangeTheme(string radioButtonName)
        {
            //если нажата радиокнопка, то согласно её названию включается нужная тема
            if (radioButtonName == "Light")
            {
                if (isMDI == false)
                {
                    //меняется вид всех существующих документов
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        tabControl1.TabPages[i].Controls[0].BackColor = Color.FromArgb(255, 255, 255);
                        tabControl1.TabPages[i].Controls[0].ForeColor = Color.Black;
                        tabControl1.TabPages[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                else 
                {
                    for (int i = 0; i < this.MdiChildren.Length; i++)
                    {
                        this.MdiChildren[i].Controls[0].BackColor = Color.FromArgb(255, 255, 255);
                        this.MdiChildren[i].Controls[0].ForeColor = Color.Black;
                        this.MdiChildren[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }              
                //меняется вид статус-бара
                statusStrip1.BackColor = Color.FromArgb(240, 240, 240);
                statusStrip1.ForeColor = Color.Black;
                currentTheme = "lightTheme";
            }
            else if (radioButtonName == "DarkRaspberry")
            {
                if (isMDI == false)
                {
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        tabControl1.TabPages[i].Controls[0].BackColor = Color.FromArgb(19, 17, 29);
                        tabControl1.TabPages[i].Controls[0].ForeColor = Color.FromArgb(241, 40, 86);
                        tabControl1.TabPages[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                else
                {
                    for (int i = 0; i < this.MdiChildren.Length; i++)
                    {
                        this.MdiChildren[i].Controls[0].BackColor = Color.FromArgb(19, 17, 29);
                        this.MdiChildren[i].Controls[0].ForeColor = Color.FromArgb(241, 40, 86);
                        this.MdiChildren[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                statusStrip1.BackColor = Color.FromArgb(241, 40, 86);
                statusStrip1.ForeColor = Color.White;
                currentTheme = "darkRaspberryTheme";
            }
            else if (radioButtonName == "DarkBlue")
            {
                if (isMDI == false)
                {
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        tabControl1.TabPages[i].Controls[0].BackColor = Color.FromArgb(40, 42, 54);
                        tabControl1.TabPages[i].Controls[0].ForeColor = Color.White;
                        tabControl1.TabPages[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                else
                {
                    for (int i = 0; i < this.MdiChildren.Length; i++)
                    {
                        this.MdiChildren[i].Controls[0].BackColor = Color.FromArgb(40, 42, 54);
                        this.MdiChildren[i].Controls[0].ForeColor = Color.White;
                        this.MdiChildren[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                statusStrip1.BackColor = Color.FromArgb(43, 52, 89);
                statusStrip1.ForeColor = Color.White;
                currentTheme = "darkBlueTheme";
            }
            else if (radioButtonName == "OceanBlue")
            {
                if (isMDI == false)
                {
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        tabControl1.TabPages[i].Controls[0].BackColor = Color.FromArgb(7, 46, 91);
                        tabControl1.TabPages[i].Controls[0].ForeColor = Color.FromArgb(167, 219, 247);
                        tabControl1.TabPages[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                else
                {
                    for (int i = 0; i < this.MdiChildren.Length; i++)
                    {
                        this.MdiChildren[i].Controls[0].BackColor = Color.FromArgb(7, 46, 91);
                        this.MdiChildren[i].Controls[0].ForeColor = Color.FromArgb(167, 219, 247);
                        this.MdiChildren[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                statusStrip1.BackColor = Color.FromArgb(1, 22, 39);
                statusStrip1.ForeColor = Color.White;
                currentTheme = "oceanBlueTheme";
            }
            else if (radioButtonName == "DarkGrey")
            {
                if (isMDI == false)
                {
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        tabControl1.TabPages[i].Controls[0].BackColor = Color.FromArgb(30, 30, 30);
                        tabControl1.TabPages[i].Controls[0].ForeColor = Color.White;
                        tabControl1.TabPages[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                else
                {
                    for (int i = 0; i < this.MdiChildren.Length; i++)
                    {
                        this.MdiChildren[i].Controls[0].BackColor = Color.FromArgb(30, 30, 30);
                        this.MdiChildren[i].Controls[0].ForeColor = Color.White;
                        this.MdiChildren[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                statusStrip1.BackColor = Color.FromArgb(45, 45, 48);
                statusStrip1.ForeColor = Color.White;
                currentTheme = "darkGreyTheme";
            }
            else if (radioButtonName == "Lime")
            {
                if (isMDI == false)
                {
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        tabControl1.TabPages[i].Controls[0].BackColor = Color.FromArgb(215, 255, 79);
                        tabControl1.TabPages[i].Controls[0].ForeColor = Color.FromArgb(42, 50, 15);
                        tabControl1.TabPages[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                else
                {
                    for (int i = 0; i < this.MdiChildren.Length; i++)
                    {
                        this.MdiChildren[i].Controls[0].BackColor = Color.FromArgb(215, 255, 79);
                        this.MdiChildren[i].Controls[0].ForeColor = Color.FromArgb(42, 50, 15);
                        this.MdiChildren[i].Controls[0].Font = new Font("Calibri", 14);
                    }
                }
                statusStrip1.BackColor = Color.FromArgb(128, 153, 47);
                statusStrip1.ForeColor = Color.White;
                currentTheme = "limeTheme";
            }
        }

        //CurrenTheme - метод, используется при нажатии на кнопку 'new' или 'open'
        //изменяет внешний вид документа в соответствии с текущей темой
        public void CurrentTheme()
        {
            if (currentTheme == "lightTheme")
            {
                tabControl1.SelectedTab.Controls[0].BackColor = Color.FromArgb(255, 255, 255);
                tabControl1.SelectedTab.Controls[0].ForeColor = Color.Black;
                tabControl1.SelectedTab.Controls[0].Font = new Font("Calibri", 14);
            }
            else if (currentTheme == "darkRaspberryTheme")
            {
                tabControl1.SelectedTab.Controls[0].BackColor = Color.FromArgb(19, 17, 29);
                tabControl1.SelectedTab.Controls[0].ForeColor = Color.FromArgb(241, 40, 86);
                tabControl1.SelectedTab.Controls[0].Font = new Font("Calibri", 14);
            }
            else if (currentTheme == "darkBlueTheme")
            {
                tabControl1.SelectedTab.Controls[0].BackColor = Color.FromArgb(40, 42, 54);
                tabControl1.SelectedTab.Controls[0].ForeColor = Color.White;
                tabControl1.SelectedTab.Controls[0].Font = new Font("Calibri", 14);
            }
            else if (currentTheme == "oceanBlueTheme")
            {
                tabControl1.SelectedTab.Controls[0].BackColor = Color.FromArgb(7, 46, 91);
                tabControl1.SelectedTab.Controls[0].ForeColor = Color.FromArgb(167, 219, 247);
                tabControl1.SelectedTab.Controls[0].Font = new Font("Calibri", 14);
            }
            else if (currentTheme == "darkGreyTheme")
            {
                tabControl1.SelectedTab.Controls[0].BackColor = Color.FromArgb(30, 30, 30);
                tabControl1.SelectedTab.Controls[0].ForeColor = Color.White;
                tabControl1.SelectedTab.Controls[0].Font = new Font("Calibri", 14);
            }
            else if (currentTheme == "limeTheme")
            {
                tabControl1.SelectedTab.Controls[0].BackColor = Color.FromArgb(215, 255, 79);
                tabControl1.SelectedTab.Controls[0].ForeColor = Color.FromArgb(42, 50, 15);
                tabControl1.SelectedTab.Controls[0].Font = new Font("Calibri", 14);
            }
        }

        //-----------------------------------------------------------------------------------------
        //Tabs/MDI

        //список текстов
        List<string> documents = new List<string>();

        //список mdi-форм (нужен для определения индекса активного окна, что бы по нему установить активной одну из вкладок)
        List<ChildForm> forms = new List<ChildForm>();

        //кол-во вкладок/окон
        int tabsCount, mdiCount; 

        //режим MDI (вкл/выкл)
        bool isMDI = false;

        //MDI mode
        private void MDIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.Clear();
            if (isMDI == false)
            {
                isMDI = true;
                pictureBox1.Visible = false;
                toolStripStatusLabel1.Visible = false;
                cascadeMDIToolStripMenuItem.Enabled = true;
                horisontalMDIToolStripMenuItem.Enabled = true;
                verticalMDIToolStripMenuItem.Enabled = true;

                //сохраняем кол-во вкладок
                tabsCount = tabControl1.TabCount;

                //копируем тексты в documents
                documents.Clear();
                if (tabControl1.TabCount != 0)
                {
                    for (int i = 0; i < tabsCount; i++)
                        documents.Add(tabControl1.TabPages[i].Controls[0].Text);
                }

                //прячем tabControl
                tabControl1.Hide();

                //включаем режим MDI
                this.IsMdiContainer = true;

                //создаем MDI окна
                for (int i = 0; i < tabsCount; i++)
                {
                    ChildForm f = new ChildForm(tabControl1.TabPages[i].Text);
                    f.MdiParent = this;
                    f.Controls[0].BackColor = tabControl1.TabPages[i].Controls[0].BackColor;
                    f.Controls[0].Font = tabControl1.TabPages[i].Controls[0].Font;
                    f.Controls[0].ForeColor = tabControl1.TabPages[i].Controls[0].ForeColor;
                    f.PerformCloseMDI += CloseMDI;
                    f.setText(documents[i]);
                    f.PerformCustomTextChanged += CustomTextChanged;
                    f.Show();
                    forms.Add(f);
                }
            }
            else
                MessageBox.Show("MDI mode has already enabled!");           
        }

        //ивент - закрытие вкладки при нажатии на закрытие окна
        private void CloseMDI()
        {
            //делаем активной ту вкладку, которая совпадает по индексу с активным mdi-окном
            if (forms.IndexOf((ChildForm)this.ActiveMdiChild) >= 0)
                tabControl1.SelectedIndex = forms.IndexOf((ChildForm)this.ActiveMdiChild);
            if (changes[tabControl1.SelectedIndex] == true)
            {
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show($"Save changes in {tabControl1.SelectedTab.Text.TrimStart('*')}?", "Saving file", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                    Save(names[tabControl1.SelectedIndex]);
            }
            names.RemoveAt(tabControl1.SelectedIndex);
            changes.RemoveAt(tabControl1.SelectedIndex);
            saves.RemoveAt(tabControl1.SelectedIndex);
            documents.RemoveAt(tabControl1.SelectedIndex);
            forms.RemoveAt(tabControl1.SelectedIndex);
            tabControl1.Controls.RemoveAt(tabControl1.SelectedIndex);
            mdiCount--;
            tabsCount--;
        }

        //ивент - пользовательское изменение текста в mdi-окне
        private void CustomTextChanged()
        {
            if (forms.IndexOf((ChildForm)this.ActiveMdiChild) >= 0)
                tabControl1.SelectedIndex = forms.IndexOf((ChildForm)this.ActiveMdiChild);
            changes[tabControl1.SelectedIndex] = true;
            if (!((ChildForm)this.ActiveMdiChild).Text.Contains("*"))
            {
                tabControl1.SelectedTab.Text = tabControl1.SelectedTab.Text.Insert(0, "*");
                ((ChildForm)this.MdiChildren[tabControl1.SelectedIndex]).Text = ((ChildForm)this.MdiChildren[tabControl1.SelectedIndex]).Text.Insert(0, "*");
            }
        }

        //TABS mode
        private void tabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isMDI == true)
            {
                isMDI = false;
                pictureBox1.Visible = true;
                toolStripStatusLabel1.Visible = true;
                cascadeMDIToolStripMenuItem.Enabled = false;
                horisontalMDIToolStripMenuItem.Enabled = false;
                verticalMDIToolStripMenuItem.Enabled = false;

                //сохраняем кол-во mdi-окон
                mdiCount = this.MdiChildren.Length;

                //копируем тексты
                documents.Clear();
                for (int i = 0; i < mdiCount; i++)
                    documents.Add(this.MdiChildren[i].Controls[0].Text);

                //показываем tabControl
                tabControl1.Show();

                //выключаем режим MDI
                this.IsMdiContainer = false;

                //заполняем вкладки
                for (int i = 0; i < mdiCount; i++)
                    tabControl1.TabPages[i].Controls[0].Text = documents[i];

                //устанавливаем тему для каждой вкладки
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    tabControl1.SelectedIndex = i;
                    CurrentTheme();
                }              
            }
            else
                MessageBox.Show("Tabs mode has already enabled!");          
        }


        //режимы расположения окон
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
    }
}
