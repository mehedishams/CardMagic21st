//////////////////////////////////////////////
//////////////////////////////////////////////
/////// Coded by Mehedi Shams Rony ///////////
//////////// 11 November 2016 ////////////////
//////////////////////////////////////////////
//////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using CardMagic21st.Classes;

namespace CardMagic21st
{
    public partial class CardForm : Form
    {
        Stack<KeyValuePair<string, object>> ShuffledCards = new Stack<KeyValuePair<string, object>>();
        List<KeyValuePair<string, object>> Cards = new List<KeyValuePair<string, object>>();
        Responsive ResponsiveObj;
        int ShuffleCount = 0;

        public CardForm()
        {
            InitializeComponent();
            ResponsiveObj = new Responsive(Screen.PrimaryScreen.Bounds);
            ResponsiveObj.SetMultiplicationFactor();

            LoadCards();
            StatusLabel.Text = "Please choose a card and click on\nthe row button where the card is!";
        }

        private void LoadCards()
        {
            // http://stackoverflow.com/questions/2041000/loop-through-all-the-resources-in-a-resx-file
            ResourceSet resourceSet = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);            

            foreach (DictionaryEntry entry in resourceSet)
                //Cards.Add(entry.Value);
                Cards.Add(new KeyValuePair<string, object>(entry.Key.ToString(), entry.Value));

            Random Rnd = new Random(DateTime.Now.Millisecond);
            int Idx;
            Hashtable HashTbl = new Hashtable();

            Control[] PicBox;
            for (int i = 0; i < 21; i++)
            {
                Idx = Rnd.Next(1, 22);
                while (ThisImageWasAlreadyTaken(--Idx, HashTbl))
                    Idx = Rnd.Next(1, 22);

                HashTbl.Add(Idx, Idx);
                PicBox = Controls.Find("PictureBox" + i, false);
                (PicBox[0] as PictureBox).Image = (Image)Cards[Idx].Value;
                ShuffledCards.Push(Cards[Idx]);
            }
        }

        private bool ThisImageWasAlreadyTaken(int Idx, Hashtable HashTbl)
        {
            return HashTbl.ContainsKey(Idx);
        }

        private void Row1Button_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, object>> OldShuffledCards = new List<KeyValuePair<string, object>>();
            foreach (KeyValuePair<string, object> KVP in ShuffledCards)
                OldShuffledCards.Add(KVP);

            ShuffledCards.Clear();
            
            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[1 + 3 * i]);
            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[2 + 3 * i]);
            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[3 * i]);
            
            ReShuffleCards();
            if (++ShuffleCount >= 3) DisplayCard();
        }

        private void ReShuffleCards()
        {
            // http://stackoverflow.com/questions/7391348/c-sharp-clone-a-stack
            Stack<KeyValuePair<string, object>> TempCards = new Stack<KeyValuePair<string, object>>(new Stack<KeyValuePair<string, object>>(ShuffledCards));
            ShuffledCards.Clear();

            Control[] PicBox;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 7; j++)
                {
                    PicBox = Controls.Find("PictureBox" + (j + (i * 7)), false);
                    KeyValuePair<string, object> KVP = TempCards.Pop();
                    ShuffledCards.Push(KVP);
                    if (ShuffleCount < 2)
                    {
                        Thread.Sleep(20);
                        (PicBox[0] as PictureBox).Image = (Image)KVP.Value;
                        Application.DoEvents();
                    }
                }

            if (ShuffleCount == 2)
            {
                Hashtable HashTbl = new Hashtable();
                int Idx;
                Random Rnd = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 7; j++)
                    {
                        PicBox = Controls.Find("PictureBox" + (j + (i * 7)), false);
                        Idx = Rnd.Next(1, 22);
                        while (ThisImageWasAlreadyTaken(--Idx, HashTbl))
                            Idx = Rnd.Next(1, 22);

                        HashTbl.Add(Idx, Idx);
                        (PicBox[0] as PictureBox).Image = (Image)Cards[Idx].Value;
                        Thread.Sleep(20);
                        Application.DoEvents();
                    }
            }
        }

        private void Row2Button_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, object>> OldShuffledCards = new List<KeyValuePair<string, object>>();
            foreach (KeyValuePair<string, object> KVP in ShuffledCards)
                OldShuffledCards.Add(KVP);

            ShuffledCards.Clear();

            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[3 * i]);
            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[1 + 3 * i]);
            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[2 + 3 * i]);

            ReShuffleCards();
            if (++ShuffleCount >= 3) DisplayCard();
        }

        private void Row3Button_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, object>> OldShuffledCards = new List<KeyValuePair<string, object>>();
            foreach (KeyValuePair<string, object> KVP in ShuffledCards)
                OldShuffledCards.Add(KVP);

            ShuffledCards.Clear();

            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[2 + 3 * i]);
            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[3 * i]);
            for (int i = 0; i < 7; i++)
                ShuffledCards.Push(OldShuffledCards[1 + 3 * i]);

            ReShuffleCards();
            if (++ShuffleCount >= 3) DisplayCard();
        }

        private void DisplayCard()
        {
            Control[] PicBox = Controls.Find("ChosenCardpictureBox", false);
            for (int i = 0; i < 10; i++)
                ShuffledCards.Pop();

            KeyValuePair<string, object> KVP = ShuffledCards.Pop();
            (PicBox[0] as PictureBox).Image = (Image)KVP.Value;

            PlayAgainButton.Visible = true;
            StatusLabel.Text = "Look I found your card!......  Magic!!";
            Row1Button.Enabled = Row2Button.Enabled = Row3Button.Enabled = false;            
        }

        private void PlayAgainButton_Click(object sender, EventArgs e)
        {
            LoadCards();
            ChosenCardpictureBox.Image = null;

            Row1Button.Enabled = Row2Button.Enabled = Row3Button.Enabled = true;
            StatusLabel.Text = "Please choose a card and click on the row button where the card is!";
            PlayAgainButton.Visible = false;
            ShuffleCount = 0;
        }

        private void CardForm_Load(object sender, EventArgs e)
        {
            Width = ResponsiveObj.GetMetrics(Width, "Width");    // Form width and height set up.
            Height = ResponsiveObj.GetMetrics(Height, "Height");
            Left = Screen.GetBounds(this).Width / 2 - Width / 2;  // Form centering.
            Top = Screen.GetBounds(this).Height / 2 - Height / 2 - 30;

            foreach (Control Ctl in this.Controls)
            {
                Ctl.Width = ResponsiveObj.GetMetrics(Ctl.Width, "Width");
                Ctl.Height = ResponsiveObj.GetMetrics(Ctl.Height, "Height");
                Ctl.Top = ResponsiveObj.GetMetrics(Ctl.Top, "Top");
                Ctl.Left = ResponsiveObj.GetMetrics(Ctl.Left, "Left");
                if (!(Ctl is PictureBox))
                    Ctl.Font = new Font(FontFamily.GenericSansSerif, ResponsiveObj.GetMetrics((int)Ctl.Font.Size), FontStyle.Regular);                    
            }
        }
    }
}