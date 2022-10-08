namespace BanglaConverter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            DisplayVowelMode();
            DisplayLanguageMode();
            HighlightActiveVowels();

            DisplayVowelMode();
            SetVowelLabels();

            KeypressProcessor.LanguageModeChangeCallback += DisplayLanguageMode;
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
            bool shift = KeypressProcessor.HasModifier(KeypressProcessor.KeyModifier.Shift);
            lblFirstVowel.BackColor = shift ? Color.White : Color.Gray;
            //lblFirstVowel.BorderStyle = shift ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
            lblA.BackColor = shift ? Color.Gray : Color.White;
            //lblA.BorderStyle = shift ? BorderStyle.FixedSingle : BorderStyle.Fixed3D;
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
            }
            else
            {
                lblFirstVowel.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.Invalid);
                lblA.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.AKar);
                lblShortI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.ShortIKar);
                lblLongI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.LongIKar);
                lblShortU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.ShortUKar);
                lblLongU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.LongUKar);
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

        private void DisplayText()
        {

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