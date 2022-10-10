namespace BanglaConverter
{
    public partial class MainForm : Form
    {

        //private readonly Color InactiveBgColor = Color.FromArgb(50, 50, 50);
        private readonly Color InactiveBgColor = Color.Gray;
        private readonly Color ActiveBgColor = Color.White;

        private readonly Color InactiveFgColor = Color.Gray;
        private readonly Color ActiveFgColor = Color.Black;

        public MainForm()
        {
            InitializeComponent();
            DisplayVowelMode();
            DisplayLanguageMode();
            HighlightActiveLetters();

            DisplayVowelMode();
            SetLetterLabels();

            KeypressProcessor.LanguageModeChangeCallback += DisplayLanguageMode;
            KeypressProcessor.LanguageModeChangeCallback += HighlightActiveLetters;

            KeypressProcessor.VowelModeChangeCallback += DisplayVowelMode;
            KeypressProcessor.VowelModeChangeCallback += SetLetterLabels;

            KeypressProcessor.KeyModifiersChangeCallback += HighlightActiveLetters;
            
            KeypressProcessor.DeliverOutput += WriteToWorkArea;
        }

        private void WriteToWorkArea(string text)
        {
            // Gets the position to insert the text.
            int cursorPosition = txtWorkArea.SelectionStart;
            // Determines if there is any selected text to write over top of.
            if (txtWorkArea.SelectionStart > 0)
            {
                // Deletes every selected character.
                txtWorkArea.Text = txtWorkArea.Text.Substring(0, cursorPosition) + txtWorkArea.Text.Substring(cursorPosition + txtWorkArea.SelectionLength);
            }
            // Inserts the new characters into the text.
            txtWorkArea.Text = txtWorkArea.Text.Substring(0, cursorPosition) + text + txtWorkArea.Text.Substring(cursorPosition);
            // Moves the cursor to the end of the inserted text.
            txtWorkArea.SelectionStart = cursorPosition + text.Length;
        }

        /// <summary>
        /// Changes the background colors of the letter labels to indicate which ones are active,
        /// depending on whether the shift key is held.
        /// </summary>
        private void HighlightActiveLetters()
        {
            // Gets the list of letters to highlight.
            List<BanglaUnicodeData.CodePoint> activeLetters = KeypressProcessor.GetActiveBanglaLetters();

            bool firstVowelActive = false, aActive = false,
                shortIActive = false, longIActive = false,
                shortUActive = false, longUActive = false,
                rActive = false, riActive = false,
                eActive = false, oiActive = false;
            
            foreach (BanglaUnicodeData.CodePoint letter in activeLetters)
            {
                if (letter == BanglaUnicodeData.CodePoint.FirstVowel)
                {
                    firstVowelActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.A || letter == BanglaUnicodeData.CodePoint.AKar)
                {
                    aActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.ShortI || letter == BanglaUnicodeData.CodePoint.ShortIKar)
                {
                    shortIActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.LongI || letter == BanglaUnicodeData.CodePoint.LongIKar)
                {
                    longIActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.ShortU || letter == BanglaUnicodeData.CodePoint.ShortUKar)
                {
                    shortUActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.LongU || letter == BanglaUnicodeData.CodePoint.LongUKar)
                {
                    longUActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.R)
                {
                    rActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.RI || letter == BanglaUnicodeData.CodePoint.RIKar)
                {
                    riActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.E || letter == BanglaUnicodeData.CodePoint.EKar)
                {
                    eActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.OI || letter == BanglaUnicodeData.CodePoint.OIKar)
                {
                    oiActive = true;
                }
            }

            //lblFirstVowel.BackColor = firstVowelActive ? ActiveBgColor : InactiveBgColor;
            //lblA.BackColor = aActive ? ActiveBgColor : InactiveBgColor;
            //lblShortI.BackColor = shortIActive ? ActiveBgColor : InactiveBgColor;
            //lblLongI.BackColor = longIActive ? ActiveBgColor : InactiveBgColor;
            //lblShortU.BackColor = shortUActive ? ActiveBgColor : InactiveBgColor;
            //lblLongU.BackColor = longUActive ? ActiveBgColor : InactiveBgColor;

            //lblFirstVowel.ForeColor = firstVowelActive ? ActiveFgColor : InactiveFgColor;
            //lblA.ForeColor = aActive ? ActiveFgColor : InactiveFgColor;
            //lblShortI.ForeColor = shortIActive ? ActiveFgColor : InactiveFgColor;
            //lblLongI.ForeColor = longIActive ? ActiveFgColor : InactiveFgColor;
            //lblShortU.ForeColor = shortUActive ? ActiveFgColor : InactiveFgColor;
            //lblLongU.ForeColor = longUActive ? ActiveFgColor : InactiveFgColor;

            lblFirstVowel.Visible = firstVowelActive;
            lblA.Visible = aActive;
            lblShortI.Visible = shortIActive;
            lblLongI.Visible = longIActive;
            lblShortU.Visible = shortUActive;
            lblLongU.Visible = longUActive;
            lblR.Visible = rActive;
            lblRI.Visible = riActive;
            lblE.Visible = eActive;
            lblOI.Visible = oiActive;
        }

        private void DisplayVowelMode()
        {
            if (KeypressProcessor.CurrentVowelMode == KeypressProcessor.VowelMode.FullVowel)
            {
                lblVowelMode.Text = "Full Vowel Mode";
            }
            else if (KeypressProcessor.CurrentVowelMode == KeypressProcessor.VowelMode.VowelSign)
            {
                lblVowelMode.Text = "Vowel Sign Mode";
            }
        }

        private void SetLetterLabels()
        {
            if (KeypressProcessor.CurrentVowelMode == KeypressProcessor.VowelMode.FullVowel)
            {
                lblFirstVowel.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.FirstVowel);
                lblA.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.A);
                lblShortI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.ShortI);
                lblLongI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.LongI);
                lblShortU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.ShortU);
                lblLongU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.LongU);
                lblRI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.RI);
                lblE.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.E);
                lblOI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.OI);
            }
            else
            {
                lblFirstVowel.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.Invalid);
                lblA.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.AKar);
                lblShortI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.ShortIKar);
                lblLongI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.LongIKar);
                lblShortU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.ShortUKar);
                lblLongU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.LongUKar);
                lblRI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.RIKar);
                lblE.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.EKar);
                lblOI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.OIKar);
            }
            lblR.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.R);
        }

        private void DisplayLanguageMode()
        {
            if (KeypressProcessor.CurrentLanguageMode == KeypressProcessor.LanguageMode.Bangla)
            {
                lblLanguageMode.Text = "Bangla Mode";
            }
            else if (KeypressProcessor.CurrentLanguageMode == KeypressProcessor.LanguageMode.English)
            {
                lblLanguageMode.Text = "English Mode";
            }    
        }

        private void txtWorkArea_KeyUp(object sender, KeyEventArgs e)
        {
            KeypressProcessor.KeyUpHandler(e);
        }

        private void txtWorkArea_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressProcessor.KeyDownHandler(e);
        }
    }
}