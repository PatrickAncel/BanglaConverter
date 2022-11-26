using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BanglaConverter
{
    public partial class SettingsForm : Form
    {

        internal static SharedData.UICallback UpdateMainForm { get; set; } = () => { };

        private Dictionary<string, Color> selectableTextColors = new Dictionary<string, Color>
        {
            ["Black"] = SystemColors.WindowText,
            ["White"] = Color.FromArgb(255, 245, 245, 240),
            ["Dark Red"] = Color.DarkRed,
            ["Blue"] = Color.Blue
        };

        private Dictionary<string, Color> selectableBackgroundColors = new Dictionary<string, Color>
        {
            ["White"] = SystemColors.Window,
            ["Black"] = Color.FromArgb(255, 25, 25, 30),
            ["Tan"] = Color.Tan
        };

        private Dictionary<string, Color> selectableFormColors = new Dictionary<string, Color>
        {
            ["Light"] = SystemColors.Control,
            ["Dark"] = Color.FromArgb(255, 20, 20, 25),
            ["Blue"] = Color.MidnightBlue
        };

        /// <summary>
        /// Adds a color to the dictionary if it is not already there.
        /// </summary>
        /// <param name="color"></param>
        private void AddToDictionaryIfMissing(Dictionary<string,Color> selectableColors, Color color)
        {
            // Checks if the color is absent from the dictionary.
            if (!selectableColors.ContainsValue(color))
            {
                // Tries to use the name of the color as the key value.
                string colorName = color.Name;

                // If the name is already in use, a suffix must be attached.
                string keySuffix = "";

                int iterationCount = 0;

                // Keeps trying new suffixes until finding an unused key name.
                while (selectableColors.ContainsKey(colorName + keySuffix))
                {
                    iterationCount++;
                    keySuffix = iterationCount.ToString();
                }

                // Adds the color to the dictionary.
                selectableColors.Add(colorName + keySuffix, color);
            }
        }

        /// <summary>
        /// Returns the index of the first string in the array that matches the name parameter,
        /// or -1 if not found.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="name"></param>
        private int GetIndex(string[] names, string name)
        {
            for (int i = 0; i < names.Length; i++)
            {
                // Checks for a match.
                if (names[i] == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private void SetUpColorComboBoxes()
        {
            // Gets the initial value of each color setting.
            Color initialTextColor = (Color)SettingsManager.GetSetting(SettingsManager.Setting.TextColor);
            Color initialBackgroundColor = (Color)SettingsManager.GetSetting(SettingsManager.Setting.BackgroundColor);
            Color initialFormColor = (Color)SettingsManager.GetSetting(SettingsManager.Setting.FormColor);

            // If any color is not found in the selectable colors, it is added.
            AddToDictionaryIfMissing(selectableTextColors, initialTextColor);
            AddToDictionaryIfMissing(selectableBackgroundColors, initialBackgroundColor);
            AddToDictionaryIfMissing(selectableFormColors, initialFormColor);

            // Clears the combo boxes so that nothing extraneous resides in them.
            cboTextColor.Items.Clear();
            cboBackgroundColor.Items.Clear();
            cboFormColor.Items.Clear();

            string[] textColorNames = selectableTextColors.Keys.ToArray();
            string[] backgroundColorNames = selectableBackgroundColors.Keys.ToArray();
            string[] formColorNames = selectableFormColors.Keys.ToArray();


            // Adds the names of the selectable colors to the combo boxes.
            cboTextColor.Items.AddRange(textColorNames);
            cboBackgroundColor.Items.AddRange(backgroundColorNames);
            cboFormColor.Items.AddRange(formColorNames);

            // Finds the dictionary key of each initial color.
            string initialTextColorName = selectableTextColors.FirstOrDefault(x => x.Value == initialTextColor).Key;
            string initialBackgroundColorName = selectableBackgroundColors.FirstOrDefault(x => x.Value == initialBackgroundColor).Key;
            string initialFormColorName = selectableFormColors.FirstOrDefault(x => x.Value == initialFormColor).Key;

            // Sets the initial values as the selected values.
            cboTextColor.SelectedIndex = GetIndex(textColorNames, initialTextColorName);
            cboBackgroundColor.SelectedIndex = GetIndex(backgroundColorNames, initialBackgroundColorName);
            cboFormColor.SelectedIndex = GetIndex(formColorNames, initialFormColorName);
        }

        public SettingsForm()
        {
            InitializeComponent();

            SetUpColorComboBoxes();

            // Gets the initial font size.
            float initialFontSize = (float)SettingsManager.GetSetting(SettingsManager.Setting.FontSize);

            numFontSize.Value = (decimal)initialFontSize;
        }

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            if (cboTextColor.SelectedItem == null)
            {
                MessageBox.Show("Please select a text color.");
                return;
            }
            if (cboBackgroundColor.SelectedItem == null)
            {
                MessageBox.Show("Please select a background color.");
                return;
            }
            if (cboFormColor.SelectedItem == null)
            {
                MessageBox.Show("Please select a form color.");
                return;
            }
            if (numFontSize.Value <= 0)
            {
                MessageBox.Show("Font size must be a positive number.");
                return;
            }
            // Updates the settings.
            SettingsManager.SetSetting(SettingsManager.Setting.TextColor, selectableTextColors[(string)cboTextColor.SelectedItem]);
            SettingsManager.SetSetting(SettingsManager.Setting.BackgroundColor, selectableBackgroundColors[(string)cboBackgroundColor.SelectedItem]);
            SettingsManager.SetSetting(SettingsManager.Setting.FormColor, selectableFormColors[(string)cboFormColor.SelectedItem]);
            SettingsManager.SetSetting(SettingsManager.Setting.FontSize, (float)numFontSize.Value);
            // Updates the main form with the new settings.
            UpdateMainForm();
        }
    }
}
