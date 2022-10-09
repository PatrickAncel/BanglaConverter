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
            HighlightActiveVowels();

            DisplayVowelMode();
            SetVowelLabels();

            KeypressProcessor.LanguageModeChangeCallback += DisplayLanguageMode;
            KeypressProcessor.LanguageModeChangeCallback += HighlightActiveVowels;

            KeypressProcessor.VowelModeChangeCallback += DisplayVowelMode;
            KeypressProcessor.VowelModeChangeCallback += SetVowelLabels;

            KeypressProcessor.KeyModifiersChangeCallback += HighlightActiveVowels;
            
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
        /// Changes the background colors of the vowel labels to indicate which ones are active,
        /// depending on whether the shift key is held.
        /// </summary>
        private void HighlightActiveVowels()
        {
            // Gets the list of vowels to highlight.
            List<BanglaUnicodeData.CodePoint> activeVowels = KeypressProcessor.GetActiveBanglaVowels();

            bool firstVowelActive = false, aActive = false,
                shortIActive = false, longIActive = false,
                shortUActive = false, longUActive = false,
                riActive = false,
                eActive = false, oiActive = false;
            
            foreach (BanglaUnicodeData.CodePoint vowel in activeVowels)
            {
                if (vowel == BanglaUnicodeData.CodePoint.FirstVowel)
                {
                    firstVowelActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.A || vowel == BanglaUnicodeData.CodePoint.AKar)
                {
                    aActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.ShortI || vowel == BanglaUnicodeData.CodePoint.ShortIKar)
                {
                    shortIActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.LongI || vowel == BanglaUnicodeData.CodePoint.LongIKar)
                {
                    longIActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.ShortU || vowel == BanglaUnicodeData.CodePoint.ShortUKar)
                {
                    shortUActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.LongU || vowel == BanglaUnicodeData.CodePoint.LongUKar)
                {
                    longUActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.RI || vowel == BanglaUnicodeData.CodePoint.RIKar)
                {
                    riActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.E || vowel == BanglaUnicodeData.CodePoint.EKar)
                {
                    eActive = true;
                }
                else if (vowel == BanglaUnicodeData.CodePoint.OI || vowel == BanglaUnicodeData.CodePoint.OIKar)
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

        private void SetVowelLabels()
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