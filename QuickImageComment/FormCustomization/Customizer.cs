//Copyright (C) 2009 Norbert Wagner

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormCustomization
{
    internal class Customizer
    {
        internal enum Texts
        {
            E_loadingConfiguration,
            I_adjustMask,
            I_close,
            I_contextMenu,
            I_imageFileNotExist,
            I_imageFileTypesAllFiles,
            I_initFileTypesAllFiles,
            I_listAssingnedShortcuts,
            I_menu,
            I_menuEntry,
            I_openFileForBackground,
            I_openSettingsFile,
            I_panelBottom,
            I_panelLeft,
            I_panelRight,
            I_panelTop,
            I_saveSettings,
            I_saveSettingsFile,
            I_saveSettingsIn,
            I_settingsWerechanged,
            I_shortcutAlreadyUsed,
            Q_changeAll,
            Q_save,
            Q_selectedElementsDifferentValues,
            Q_shallBeSaved,
            Q_shallBeSavedInSettingsFile,
            W_invalidShortCutValidAre1,
            W_invalidShortCutValidAre2,
            W_invalidShortCutValidAre3,
            W_noValidShortcut
        }

        // with 4.54 a general zoom factor was introduced
        // form specific zoom factors are only written to file, if the differ from general factor
        // as old configuration files always contained an entry for zoom factor, the name was changed
        // to detect old entries, where a value of 1 is considered as "not set"
        private const string PropertyNameZoomOld = "_ZoomFactor";
        private const string PropertyNameZoom = "_ZoomFactor2";
        private const int zoomOffsetWidth = 16;
        private const int zoomOffsetHeight = 39;
        private bool customizedSettingChanged;
        private static SortedList<Texts, string> GermanTexts = new SortedList<Texts, string>();
        private static ArrayList UsedTranslations = new ArrayList();
        private static ArrayList NotTranslatedTexts = new ArrayList();

        private static float generalZoomFactor = 1f;
        private static SortedList<string, int> NewFontSizesForZoom = new SortedList<string, int>();
        private const string fontSizeTest = "MlMlMlMlM";

        // as controls can be moved between different panels, the leading part of control's full name
        // shall be ignored, when adding them in zoom basis data collection
        // sequence must be in a way, that no entry is contained in a following one (i.e. "abc" before "ab")
        internal static string[] leadingControlNamePartsToIgnore;
        internal static string[] leadingControlNamePartsPrefixDollar;

        // contains properties of components
        private class PropertyPair
        {
            public object Original;
            public object Customized;

            public PropertyPair(object givenOriginal, object givenCustomized)
            {
                Original = givenOriginal;
                Customized = givenCustomized;
            }
        }

        // class contains data before first zoom
        private class ZoomBasisData
        {
            public float Top;
            public float Left;
            public float Width;
            public float Height;
            public float FontSize;
            public float minHeight;
            public float minWidth;
            public float SplitterDistance = 0;
            public float Panel1MinSize = 0;
            public float Panel2MinSize = 0;
            public float ItemSizeWidth = 0;
            public float ItemSizeHeight = 0;
            public float RowTemplateHeight = 0;
            public Size ImageScalingSize;
            public bool noGapRight = false;
            public bool noGapBottom = false;

            public ZoomBasisData(Control givenControl)
            {
                Top = givenControl.Top;
                Left = givenControl.Left;
                Width = givenControl.Width;
                Height = givenControl.Height;
                FontSize = givenControl.Font.Size;
                minHeight = givenControl.MinimumSize.Height;
                minWidth = givenControl.MinimumSize.Width;
                if (givenControl is SplitContainer) SplitterDistance = ((SplitContainer)givenControl).SplitterDistance;
                if (givenControl is ToolStrip) ImageScalingSize = ((ToolStrip)givenControl).ImageScalingSize;
                if (givenControl.Parent != null && (givenControl.Anchor & AnchorStyles.Right) == AnchorStyles.Right
                    && givenControl.Right + givenControl.Width == givenControl.Parent.Width) noGapRight = true;
                if (givenControl.Parent != null && (givenControl.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom
                    && givenControl.Top + givenControl.Height == givenControl.Parent.Height) noGapBottom = true;
            }
        }

        // name of last loaded or saved customization settings file
        private string lastCustomizationFile = "";
        // header line for customization settings file
        private string FileHeaderLine = "";
        // URL for help file
        private string HelpUrl = "";
        // HelpTopic to show when using menu entry "Help"
        private string HelpTopic = "";
        private SortedList<string, float> ActualZoomFactors = new SortedList<string, float>();
        // holds changed properties and their orignal values
        private Hashtable PropertyTable = new Hashtable();
        private bool PropertyTableContainsComponentSettings = false;
        // holds zoom basis data (before first zoom)
        private Hashtable ZoomBasisDataTable = new Hashtable();
        // holds shortcuts for controls (not ToolStripMenuItems)
        private Hashtable ShortcutsForHandler = new Hashtable();
        // holds pairs of Strings for translation
        private static SortedList<string, string> Translations;

        // enum to change properties of form components
        // when adding new enumProperty, adjust also:
        // PropertyNames, getProperty, set
        internal enum enumProperty
        {
            BackColor, ForeColor, Font, Left, Top, Width, Height,
            TabIndex, Text, BackgroundImage, AutoSize, Shortcut
        };
        private string[] PropertyNames =
          new string[] { "BackColor", "ForeColor", "Font", "Left", "Top", "Width", "Height",
        "TabIndex", "Text", "BackgroundImage", "AutoSize", "Shortcut" };

        // enum to select original or customized for setting form components
        internal enum enumSetTo { Original, Customized };

        // constructor
        internal Customizer(string givenFileHeaderLine, string givenHelpUrl, string givenHelpTopic,
            SortedList<string, string> givenTranslations)
        {
            customizedSettingChanged = false;
            FileHeaderLine = givenFileHeaderLine;
            HelpUrl = givenHelpUrl;
            HelpTopic = givenHelpTopic;
            Translations = givenTranslations;

            GermanTexts.Add(Texts.E_loadingConfiguration, "Fehler beim Laden der Masken-Konfiguration.");
            GermanTexts.Add(Texts.I_adjustMask, "Maske \"\\1\" anpassen");
            GermanTexts.Add(Texts.I_close, "Schließen");
            GermanTexts.Add(Texts.I_contextMenu, "Kontextmenü");
            GermanTexts.Add(Texts.I_imageFileNotExist, "Bilddatei nicht vorhanden:");
            GermanTexts.Add(Texts.I_imageFileTypesAllFiles, "Bilddateien (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|Alle Dateien (*.*)|*.*");
            GermanTexts.Add(Texts.I_initFileTypesAllFiles, "Initialisierungsdateien (*.ini)|*.ini|Alle Dateien (*.*)|*.*");
            GermanTexts.Add(Texts.I_listAssingnedShortcuts, "Liste zugeordneter Tastaturkürzel");
            GermanTexts.Add(Texts.I_menu, "Menü");
            GermanTexts.Add(Texts.I_menuEntry, "Menüeintrag \"\\1\"");
            GermanTexts.Add(Texts.I_openFileForBackground, "Datei für Hintergrundbild öffnen");
            GermanTexts.Add(Texts.I_openSettingsFile, "Einstellungs-Datei öffnen");
            GermanTexts.Add(Texts.I_panelBottom, "Panel unten");
            GermanTexts.Add(Texts.I_panelLeft, "Panel links");
            GermanTexts.Add(Texts.I_panelRight, "Panel rechts");
            GermanTexts.Add(Texts.I_panelTop, "Panel oben");
            GermanTexts.Add(Texts.I_saveSettings, "Einstellungen speichern");
            GermanTexts.Add(Texts.I_saveSettingsFile, "Einstellungs-Datei speichern");
            GermanTexts.Add(Texts.I_saveSettingsIn, "Einstellungen speichern in \\1");
            GermanTexts.Add(Texts.I_settingsWerechanged, "Einstellungen zur Anpassung von Masken wurden geändert.");
            GermanTexts.Add(Texts.I_shortcutAlreadyUsed, "Tastaturkürzel \"\\1\" wird bereits für \"\\2\" verwendet.");
            GermanTexts.Add(Texts.Q_changeAll, "Alle ändern?");
            GermanTexts.Add(Texts.Q_save, "Speichern?");
            GermanTexts.Add(Texts.Q_selectedElementsDifferentValues, "Ausgewählte Elemente haben unterschiedliche Werte (\\1-\\2). Alle auf neuen Wert setzen?");
            GermanTexts.Add(Texts.Q_shallBeSaved, "Sollen sie in \"\\1\" gespeichert werden?");
            GermanTexts.Add(Texts.Q_shallBeSavedInSettingsFile, "Sollen sie in einer Einstellungs-Datei gespeichert werden?");
            GermanTexts.Add(Texts.W_invalidShortCutValidAre1, "Keine zulässige Tastenkombination. Zulässig sind");
            GermanTexts.Add(Texts.W_invalidShortCutValidAre2, "- Funktionstasten, z.B. F8, F11");
            GermanTexts.Add(Texts.W_invalidShortCutValidAre3, "- Tastenkombinationen mit Strg (Ctrl)");
            GermanTexts.Add(Texts.W_noValidShortcut, "\"\\1\" ist kein gültiges Tastaturkürzel.");
        }

        //*****************************************************************
        #region general internal methods
        //*****************************************************************

        // get the name of last loaded or saved customization settings file
        internal string getLastCustomizationFile()
        {
            return lastCustomizationFile;
        }
        // get the settings for help
        internal string getHelpUrl()
        {
            return HelpUrl;
        }
        internal string getHelpTopic()
        {
            return HelpTopic;
        }

        // clear name of last loaded or saved customization settings file
        internal void clearLastCustomizationFile()
        {
            lastCustomizationFile = "";
        }

        // get zoom factor to be applied (general or form specific)
        internal float getTargetZoomFactor(Form theForm)
        {
            string zoomKey = theForm.Name + ":" + PropertyNameZoom;
            if (PropertyTable.ContainsKey(zoomKey))
            {
                return (float)((PropertyPair)PropertyTable[zoomKey]).Customized;
            }
            else
            {
                return generalZoomFactor;
            }
        }

        // get factor which was used to zoom form)
        internal float getActualZoomFactor(Form theForm)
        {
            if (ActualZoomFactors.ContainsKey(theForm.Name))
            {
                return ActualZoomFactors[theForm.Name];
            }
            else
            {
                return 1;
            }
        }

        // set list of translations
        internal static void setTranslations(SortedList<string, string> givenTranslations)
        {
            Translations = givenTranslations;
        }

        internal static ArrayList getUsedTranslations()
        {
            string translation;
            foreach (string text in GermanTexts.Values)
            {
                // use this also to check if translations are available
                // will show messagebox in case of error
                translation = translate(text);

                UsedTranslations.Add(text);
            }
            return UsedTranslations;
        }

        internal static ArrayList getNotTranslatedTexts()
        {
            return NotTranslatedTexts;
        }

        // get text via key, if needed translate 
        internal static string getText(Texts key)
        {
            return translate(GermanTexts[key]);
        }
        internal static string getText(Texts key, string Parameter1)
        {
            return translate(GermanTexts[key], Parameter1);
        }
        internal static string getText(Texts key, string Parameter1, string Parameter2)
        {
            return translate(GermanTexts[key], Parameter1, Parameter2);
        }

        // translate a text and issue error message if text not found in SortedList translations
        internal static string translate(string TextToTranslate)
        {
            if (Translations.ContainsKey(TextToTranslate))
            {
                UsedTranslations.Add(TextToTranslate);
                return Translations[TextToTranslate];
            }
            else
            {
                if (Translations.Count > 0)
                {
                    // Translations are available, but not for this text
                    NotTranslatedTexts.Add(TextToTranslate);
                }
                return TextToTranslate;
            }
        }

        // translate a text and issue error message if text not found in SortedList translations - with one parameter
        internal static string translate(string TextToTranslate, string Parameter1)
        {
            string Temp = translate(TextToTranslate);
            Temp = Temp.Replace("\\1", Parameter1);
            return Temp;
        }

        // translate a text and issue error message if text not found in SortedList translations - with two parameters
        internal static string translate(string TextToTranslate, string Parameter1, string Parameter2)
        {
            string Temp = translate(TextToTranslate);
            Temp = Temp.Replace("\\1", Parameter1);
            Temp = Temp.Replace("\\2", Parameter2);
            return Temp;
        }

        // set properties of all form components based on original settings
        // remove zoom basis data and zoom
        // when same form is created again, old zoom data are still in Customizer,
        // so they are deleted in order not to zoom based on wrong data
        // to be called before .Show (as controls are not hidden during modification)
        internal void setAllComponentsZoomInitial(enumSetTo SetTo, Form theForm)
        {
            if (ActualZoomFactors.ContainsKey(theForm.Name))
            {
                ActualZoomFactors.Remove(theForm.Name);
            }
            removeZoomBasisData(theForm);
            setAllComponents(SetTo, theForm, false);
        }

        // set properties of all form components based on original settings
        // zoom only if zoom factor changed
        internal void setAllComponentsZoomIfChanged(enumSetTo SetTo, Form theForm)
        {
            setAllComponents(SetTo, theForm, true);
        }

        // set properties of all form components based on original settings
        // zoom only if zoom factor changed; no hide of controls during modification
        internal void setAllComponentsZoomIfChangedNoHideDuringModfication(enumSetTo SetTo, Form theForm)
        {
            setAllComponents(SetTo, theForm, false);
        }

        // zoom the form and set properties of all form components based on original settings
        internal void setAllComponents(enumSetTo SetTo, Form theForm, bool hideControlsDuringModification)
        {
            string ComponentFullName;
            string PropertyName;
            enumProperty propertyIndex;
            int indexColon;

            float OldZoomFactor = getActualZoomFactor(theForm);

            // determine new individual zoom factor
            float NewIndividualZoomFactor = 0f;

            string zoomKey = theForm.Name + ":" + PropertyNameZoom;
            if (PropertyTable.ContainsKey(zoomKey))
            {
                if (SetTo == enumSetTo.Original)
                {
                    // keep value of 0f which means not specified
                    // thus zoomForm will use general zoom factor
                }
                else if (SetTo == enumSetTo.Customized)
                {
                    NewIndividualZoomFactor = (float)((PropertyPair)PropertyTable[zoomKey]).Customized;
                }
                else
                {
                    throw new Exception("Internal Error");
                }
            }

            float NewZoomFactor;

            if (NewIndividualZoomFactor > 0f)
            {
                // individual zoom factor is given
                NewZoomFactor = NewIndividualZoomFactor;

                // save new zoom factor in property table
                if (PropertyTable.ContainsKey(zoomKey))
                {
                    // Original value is set when first zoom is triggered
                    // thus original value is used for proper initialisation
                    if (((PropertyPair)PropertyTable[zoomKey]).Original == null)
                    {
                        ((PropertyPair)PropertyTable[zoomKey]).Original = OldZoomFactor;
                    }
                    ((PropertyPair)PropertyTable[zoomKey]).Customized = NewZoomFactor;
                }
                else
                {
                    // property not in list, add it customized value, original value not used here
                    PropertyTable.Add(zoomKey, new PropertyPair(null, NewZoomFactor));
                }
            }
            else
            {
                // no individual factor given, use general zoom factor
                NewZoomFactor = generalZoomFactor;
            }

            bool hideAllControls = hideControlsDuringModification && (NewZoomFactor != OldZoomFactor || PropertyTableContainsComponentSettings);

            if (hideAllControls)
            {
                foreach (Control control in theForm.Controls) control.Visible = false;
            }

            if (NewZoomFactor != OldZoomFactor)
            {
                // zoom the form
                zoomForm(SetTo, theForm, NewZoomFactor);
            }

            // in case property table contains only form specific zoom factors, following block can be skipped
            if (PropertyTableContainsComponentSettings)
            {
                // adjust all form components
                // first create sorted list of keys
                // the sort order ensures that first childs are changed, then parents
                // this is important in case childs inherit properties from parents, 
                // then the child's original must not be changed because parent is changed before
                ArrayList SortedKeys = new ArrayList(PropertyTable.Keys);
                SortedKeys.Sort();

                SortedList<string, Component> sortedListComponents = new SortedList<string, Component>();
                fillSortedListComponents(sortedListComponents, theForm);

                foreach (string Key in SortedKeys)
                {
                    indexColon = Key.IndexOf(":");
                    ComponentFullName = Key.Substring(0, indexColon);
                    PropertyName = Key.Substring(indexColon + 1);
                    if (!PropertyName.Equals(PropertyNameZoom))
                    {
                        string componentKey = removeSpecificLeadingPartsControlFullNamePrefixDollar(ComponentFullName);

                        if (sortedListComponents.ContainsKey(componentKey))
                        {
                            Component ChangeableComponent = sortedListComponents[componentKey];
                            propertyIndex = getPropertyIndex(PropertyName);

                            if (SetTo == enumSetTo.Original)
                            {
                                setProperty(ChangeableComponent, propertyIndex, ((PropertyPair)PropertyTable[Key]).Original);
                                ((PropertyPair)PropertyTable[Key]).Customized = null;
                                customizedSettingChanged = true;
                            }
                            else if (SetTo == enumSetTo.Customized)
                            {
                                if (((PropertyPair)PropertyTable[Key]).Original == null)
                                {
                                    ((PropertyPair)PropertyTable[Key]).Original = getProperty(ChangeableComponent, propertyIndex);
                                }
                                // due to changing settings property table might have entries 
                                // whose customized value has been cleared
                                if (((PropertyPair)PropertyTable[Key]).Customized != null)
                                {
                                    setProperty(ChangeableComponent, propertyIndex, ((PropertyPair)PropertyTable[Key]).Customized);
                                }
                            }
                            else
                            {
                                throw new Exception("Internal Error");
                            }
                        }
                    }
                }
            }
            if (hideAllControls)
            {
                foreach (Control control in theForm.Controls) control.Visible = true;
            }

            //foreach (string key in ZoomBasisDataTable.Keys) Logger.log("--" + key);
        }

        private void fillSortedListComponents(SortedList<string, Component> sortedListControls, Component parentComponent)
        {
            string fullName = getFullNameOfComponent(parentComponent);
            // some controls have "subcontrols" without name, they would lead to duplicate keys, but can be ignored
            if (!sortedListControls.ContainsKey(fullName))
            {
                sortedListControls.Add(fullName, parentComponent);
            }

            if (parentComponent is MenuStrip)
            {
                MenuStrip menuStrip = (MenuStrip)parentComponent;
                foreach (Component item in menuStrip.Items)
                {
                    fillSortedListComponents(sortedListControls, item);
                }
            }
            else if (parentComponent is ToolStripMenuItem)
            {
                foreach (Component item in ((ToolStripMenuItem)parentComponent).DropDownItems)
                {
                    fillSortedListComponents(sortedListControls, item);
                }
            }
            else if (parentComponent is ToolStripDropDownButton)
            {
                foreach (Component item in ((ToolStripDropDownButton)parentComponent).DropDownItems)
                {
                    fillSortedListComponents(sortedListControls, item);
                }
            }
            else if (parentComponent is Control)
            {
                foreach (Control childComponent in ((Control)parentComponent).Controls)
                {
                    fillSortedListComponents(sortedListControls, childComponent);
                }
            }
        }

        // clear flag indicating that customized settings were changed
        internal void clearCustomizedSettingsChanged()
        {
            customizedSettingChanged = false;
        }

        // set flag indicating that customized settings were changed
        internal void setCustomizedSettingsChanged()
        {
            customizedSettingChanged = true;
        }

        // if settings are changed and saving is confirmed, save settings
        internal void saveIfChangedAndConfirmed()
        {
            if (customizedSettingChanged)
            {
                DialogResult theDialogResult;
                if (lastCustomizationFile.Equals(""))
                {
                    theDialogResult = MessageBox.Show(Customizer.getText(Texts.I_settingsWerechanged)
                        + "\n" + Customizer.getText(Texts.Q_shallBeSavedInSettingsFile),
                        Customizer.getText(Texts.Q_save), MessageBoxButtons.YesNo);
                    if (theDialogResult == DialogResult.Yes)
                    {
                        writeCustomizationFile();
                    }
                }
                else
                {
                    theDialogResult = MessageBox.Show(Customizer.getText(Texts.I_settingsWerechanged)
                        + "\n" + Customizer.getText(Texts.Q_shallBeSaved, lastCustomizationFile),
                        Customizer.getText(Texts.Q_save), MessageBoxButtons.YesNo);
                    if (theDialogResult == DialogResult.Yes)
                    {
                        writeCustomizationFile(lastCustomizationFile);
                    }
                }
            }
        }

        // show mask with list of keys
        internal void showListOfKeys(Form theForm)
        {
            ArrayList ShortcutKeys = new ArrayList();
            ArrayList ShortcutDescriptions = new ArrayList();
            fillListOfShortcuts(theForm, ShortcutKeys, ShortcutDescriptions);
            FormListOfKeys theFormListOfKeys = new FormListOfKeys(theForm, ShortcutKeys, ShortcutDescriptions, this);
        }

        // returns Description of key if key is contained in list of shortcut keys, else empty string
        internal string ShortcutKeysListContains(Form theForm, string KeyString)
        {
            ArrayList ShortcutKeys = new ArrayList();
            ArrayList ShortcutDescriptions = new ArrayList();
            fillListOfShortcuts(theForm, ShortcutKeys, ShortcutDescriptions);
            int ii = ShortcutKeys.IndexOf(KeyString);
            if (ii < 0)
            {
                return "";
            }
            else
            {
                return (string)ShortcutDescriptions[ii];
            }
        }
        #endregion

        //*****************************************************************
        #region zoom the form
        //*****************************************************************

        // zoom a form and all its components
        internal void zoomForm(enumSetTo SetTo, Form zoomableForm, float NewZoomFactor)
        {
            float OldZoomFactor = getActualZoomFactor(zoomableForm);

            zoomableForm.SuspendLayout();

            fillOrUpdateZoomBasisData(zoomableForm, OldZoomFactor);
            zoomControls(zoomableForm, NewZoomFactor);

            zoomableForm.ResumeLayout();

            // store factor as actual applied zoom factor
            if (ActualZoomFactors.ContainsKey(zoomableForm.Name))
            {
                ActualZoomFactors[zoomableForm.Name] = NewZoomFactor;
            }
            else
            {
                ActualZoomFactors.Add(zoomableForm.Name, NewZoomFactor);
            }
        }

        // fill the hashtable with zoom basis data of the control and its childs
        internal void fillOrUpdateZoomBasisData(Control ParentControl, float actualZoomFactor)
        {
            addOrUpdateZoomBasisData(ParentControl, actualZoomFactor);
            foreach (Control ChildControl in ParentControl.Controls)
            {
                // do not add markup panels, name not unique and panels are varying
                if (!(ChildControl is Panel && ChildControl.Name.Equals("_MARKUP_PANEL_")))
                {
                    fillOrUpdateZoomBasisData(ChildControl, actualZoomFactor);
                }
            }
        }

        // remove entries in hashtable with zoom basis data of the control and its childs
        internal void removeZoomBasisData(Control ParentControl)
        {
            string theControlFullName = getFullNameOfComponent(ParentControl);
            if (ZoomBasisDataTable.ContainsKey(theControlFullName))
            {
                ZoomBasisDataTable.Remove(theControlFullName);
            }
            foreach (Control ChildControl in ParentControl.Controls)
            {
                removeZoomBasisData(ChildControl);
            }
        }

        // add or overwrite zoom basis data for one control
        private void addOrUpdateZoomBasisData(Control theControl, float actualZoomFactor)
        {
            string theControlFullName = getFullNameOfComponent(theControl);
            if (ZoomBasisDataTable.ContainsKey(theControlFullName))
            {
                // basis data already available, update considering actual zoom factor
                ZoomBasisData zoomBasisData = (ZoomBasisData)ZoomBasisDataTable[theControlFullName];

                if (theControl is Form)
                {
                    zoomBasisData.Width = (theControl.Width - zoomOffsetWidth) / actualZoomFactor + zoomOffsetWidth;
                    zoomBasisData.Height = (theControl.Height - zoomOffsetHeight) / actualZoomFactor + zoomOffsetHeight;
                }
                else
                {
                    // depending on Anchor, position and size may have been changed due to 
                    // size change of mask or moving splitter
                    if ((theControl.Anchor & AnchorStyles.Left) != AnchorStyles.Left)
                    {
                        // control has no anchor Left: moving splitter will change Left
                        zoomBasisData.Left = theControl.Left / actualZoomFactor;
                    }
                    if ((theControl.Anchor & AnchorStyles.Left) == AnchorStyles.Left &&
                        (theControl.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
                    {
                        // control has anchor Left and Right: moving splitter will change width
                        zoomBasisData.Width = theControl.Width / actualZoomFactor;
                    }

                    if ((theControl.Anchor & AnchorStyles.Top) != AnchorStyles.Top)
                    {
                        zoomBasisData.Top = theControl.Top / actualZoomFactor;
                    }
                    if ((theControl.Anchor & AnchorStyles.Top) == AnchorStyles.Top &&
                        (theControl.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                    {
                        zoomBasisData.Height = theControl.Height / actualZoomFactor;
                    }

                    if (theControl is SplitContainer)
                    {
                        zoomBasisData.SplitterDistance = ((SplitContainer)theControl).SplitterDistance / actualZoomFactor;
                        zoomBasisData.Panel1MinSize = ((SplitContainer)theControl).Panel1MinSize / actualZoomFactor;
                        zoomBasisData.Panel2MinSize = ((SplitContainer)theControl).Panel2MinSize / actualZoomFactor;
                    }
                }
            }
            else
            {
                // add basis data
                ZoomBasisData zoomBasisData = new ZoomBasisData(theControl);

                ZoomBasisDataTable.Add(theControlFullName, zoomBasisData);

                if (theControl is DataGridView)
                {
                    ((ZoomBasisData)ZoomBasisDataTable[theControlFullName]).RowTemplateHeight =
                        ((DataGridView)theControl).RowTemplate.Height;
                }
                else if (theControl is TabControl)
                {
                    ((ZoomBasisData)ZoomBasisDataTable[theControlFullName]).ItemSizeHeight =
                        ((TabControl)theControl).ItemSize.Height;
                    ((ZoomBasisData)ZoomBasisDataTable[theControlFullName]).ItemSizeWidth =
                        ((TabControl)theControl).ItemSize.Width;
                }
            }
        }

        // zoom controls including childs using actual zoom factor (general or form specific)
        // actual zoom factor is used for update zoom basis data (if required)
        internal void zoomControlsUsingTargetZoomFactor(Control ParentControl, Form ContainingForm)
        {
            // method can be called when zoom basis data are not yet filled
            fillOrUpdateZoomBasisData(ParentControl, getActualZoomFactor(ContainingForm));
            zoomControls(ParentControl, getTargetZoomFactor(ContainingForm));
        }

        // zoom controls including childs
        internal void zoomControls(Control ParentControl, float zoomFactor)
        {
            string ParentControlFullName = getFullNameOfComponent(ParentControl);

            // get the zoom basis data
            ZoomBasisData theZoomBasisData = (ZoomBasisData)ZoomBasisDataTable[ParentControlFullName];

            if (theZoomBasisData != null)
            {
                // reset minimum size to avoid that minimum size prevents changing size
                ParentControl.MinimumSize = new Size(0, 0);

                if (ParentControl is SplitContainer)
                {
                    ((SplitContainer)ParentControl).Panel1MinSize = 0;
                    ((SplitContainer)ParentControl).Panel2MinSize = 0;
                }

                // get new font size by trying which font size fits in zoomed size of control
                ParentControl.Font = getZoomedFont(ParentControl.Font, theZoomBasisData.FontSize, zoomFactor);

                // in order to take effect, this needs to be done before changing size (at least when it is first time)
                if (ParentControl is ToolStrip)
                {
                    ((ToolStrip)ParentControl).ImageScalingSize = new Size((int)(theZoomBasisData.ImageScalingSize.Width * zoomFactor),
                                                                           (int)(theZoomBasisData.ImageScalingSize.Height * zoomFactor));
                }

                if (ParentControl is Form)
                {
                    // zoom only inner part of form (not the borders)
                    ParentControl.Width = (int)((theZoomBasisData.Width - zoomOffsetWidth) * zoomFactor + zoomOffsetWidth);
                    ParentControl.Height = (int)((theZoomBasisData.Height - zoomOffsetHeight) * zoomFactor + zoomOffsetHeight);
                }
                else
                {
                    // deactivate AutoSize for menu strip and status strip to ensure that control changes size
                    // for other controls it can have the effect, that text is truncated
                    if (ParentControl is MenuStrip || ParentControl is StatusStrip || ParentControl is ToolStrip)
                    {
                        ParentControl.AutoSize = false;
                    }
                    if (theZoomBasisData.noGapRight && ParentControl.Parent != null)
                        ParentControl.Width = ParentControl.Parent.Width - ParentControl.Right;
                    else
                        ParentControl.Width = (int)(theZoomBasisData.Width * zoomFactor);
                    if (theZoomBasisData.noGapBottom && ParentControl.Parent != null)
                        ParentControl.Height = ParentControl.Parent.Height - ParentControl.Top;
                    else
                        ParentControl.Height = (int)(theZoomBasisData.Height * zoomFactor);
                }

                if (ParentControl is DataGridView)
                {
                    ((DataGridView)ParentControl).RowTemplate.Height = (int)(theZoomBasisData.RowTemplateHeight * zoomFactor);
                }
                else if (ParentControl is TabControl)
                {
                    ((TabControl)ParentControl).ItemSize =
                        new Size((int)(theZoomBasisData.ItemSizeWidth * zoomFactor) + 20, (int)(theZoomBasisData.ItemSizeHeight * zoomFactor));
                }
                else if (ParentControl is DateTimePicker)
                {
                    // note: the DateTimePicker out of the box does not consider font changes
                    // see https://stackoverflow.com/questions/48020286/is-it-possible-to-increase-size-of-calendar-popup-in-winform
                    ((DateTimePicker)ParentControl).CalendarFont = getZoomedFont(((DateTimePicker)ParentControl).CalendarFont,
                        theZoomBasisData.FontSize, zoomFactor);
                }

                // do not change position of form
                if (!(ParentControl is Form))
                {
                    ParentControl.Left = (int)(theZoomBasisData.Left * zoomFactor);
                    ParentControl.Top = (int)(theZoomBasisData.Top * zoomFactor);
                }

                // set new minimum size considering zoom factor
                int minWidth = 0;
                int minHeight = 0;
                if (ParentControl is Form)
                {
                    minWidth = (int)((theZoomBasisData.minWidth - zoomOffsetWidth) * zoomFactor + zoomOffsetWidth);
                    minHeight = (int)((theZoomBasisData.minHeight - zoomOffsetHeight) * zoomFactor + zoomOffsetHeight);
                }
                else
                {
                    minWidth = (int)(theZoomBasisData.minWidth * zoomFactor);
                    minHeight = (int)(theZoomBasisData.minHeight * zoomFactor);
                }
                if (minWidth > 0 && minHeight > 0) ParentControl.MinimumSize = new Size(minWidth, minHeight);

                if (ParentControl is SplitContainer)
                {
                    ((SplitContainer)ParentControl).Panel1MinSize = (int)(theZoomBasisData.Panel1MinSize * zoomFactor);
                    ((SplitContainer)ParentControl).Panel2MinSize = (int)(theZoomBasisData.Panel2MinSize * zoomFactor);
                    int newSplitterDistance = (int)(theZoomBasisData.SplitterDistance * zoomFactor);
                    ((SplitContainer)ParentControl).SplitterDistance = newSplitterDistance;
                }

                // zoom the child controls
                if (ParentControl is StatusStrip)
                {
                    foreach (ToolStripStatusLabel ChildControl in ((StatusStrip)ParentControl).Items)
                    {
                        ChildControl.Height = (int)(ChildControl.Height * zoomFactor);
                        ChildControl.Width = (int)(ChildControl.Width * zoomFactor);
                    }
                }
                else
                {
                    foreach (Control ChildControl in ParentControl.Controls)
                    {
                        // do not try to zoom markup panels; no zoom basis data available for them
                        if (!(ChildControl is Panel && ChildControl.Name.Equals("_MARKUP_PANEL_")))
                        {
                            zoomControls(ChildControl, zoomFactor);
                        }
                    }
                }
                if (ParentControl.ContextMenuStrip != null)
                {
                    // Context menu strip may have a different font, but for simplicity just use the control's font
                    ParentControl.ContextMenuStrip.Font = ParentControl.Font;
                }
            }
        }

        #endregion

        //*****************************************************************
        #region methods for key handling
        //*****************************************************************

        // add or overwrite the shortcut in table
        internal void addOverwriteShortcutInTable(string ShortcutKeyString, Control theControl)
        {
            if (ShortcutKeyString.Equals(""))
            {
                foreach (string aShortcut in ShortcutsForHandler.Keys)
                {
                    if (ShortcutsForHandler[aShortcut].Equals(theControl))
                    {
                        ShortcutsForHandler.Remove(aShortcut);
                        break;
                    }
                }
            }
            else if (ShortcutsForHandler.ContainsKey(ShortcutKeyString.ToString()))
            {
                ShortcutsForHandler[ShortcutKeyString] = theControl;
            }
            else
            {
                ShortcutsForHandler.Add(ShortcutKeyString, theControl);
            }
        }

        // general event handler for key down
        internal void Form_KeyDown(object sender, KeyEventArgs e)
        {
            string ShortcutKeyString = ((Form)sender).Name + "." + e.KeyData.ToString();
            Control assignedControl = (Control)ShortcutsForHandler[ShortcutKeyString];
            if (assignedControl is Button)
            {
                ((Button)assignedControl).PerformClick();
            }
            else if (assignedControl is TabPage)
            {
                TabControl theTabControl = (TabControl)((TabPage)assignedControl).Parent;
                theTabControl.SelectTab((TabPage)assignedControl);
            }
            else if (assignedControl != null)
            {
                assignedControl.Select();
            }
        }

        // fill lists of Shortcuts, keys and description
        private void fillListOfShortcuts(Form theForm, ArrayList ShortcutKeys, ArrayList ShortcutDescriptions)
        {
            ShortcutKeys.Clear();
            ShortcutDescriptions.Clear();
            foreach (Control aControl in theForm.Controls)
            {
                addShortcutsInList(ShortcutKeys, ShortcutDescriptions, aControl, "");
            }
            foreach (string KeyString in ShortcutsForHandler.Keys)
            {
                if (KeyString.StartsWith(theForm.Name))
                {
                    ShortcutKeys.Add(KeyString.Substring(theForm.Name.Length + 1));
                    Control theControl = (Control)ShortcutsForHandler[KeyString];
                    ShortcutDescriptions.Add(theControl.Name + " \"" + theControl.Text + "\"");
                }
            }
        }

        // recursively add shortcuts of controls to list of shortcuts
        private void addShortcutsInList(ArrayList ShortcutKeys, ArrayList ShortcutDescriptions,
          Component parentComponent, string parentMenuString)
        {
            // when adding new types: search for add-new-type-here to find 
            // all other locations where changes are necessary!!!
            if (parentComponent is MenuStrip)
            {
                foreach (Component aMenuItem in ((MenuStrip)parentComponent).Items)
                {
                    addShortcutsInList(ShortcutKeys, ShortcutDescriptions, aMenuItem, Customizer.getText(Texts.I_menu) + ": ");
                }
            }
            else if (parentComponent is ContextMenuStrip)
            {
                foreach (ToolStripMenuItem aMenuItem in ((ContextMenuStrip)parentComponent).Items)
                {
                    string MenuString = Customizer.getText(Texts.I_contextMenu) + " " + ((ContextMenuStrip)parentComponent).Text + ":";
                    addShortcutsInList(ShortcutKeys, ShortcutDescriptions, aMenuItem, MenuString);
                }
            }
            else if (parentComponent is ToolStripDropDownButton)
            {
                foreach (Component aMenuItem in ((ToolStripDropDownButton)parentComponent).DropDownItems)
                {
                    string MenuString = Customizer.getText(Texts.I_contextMenu) + " " + ((ToolStripDropDownButton)parentComponent).Text + ":";
                    addShortcutsInList(ShortcutKeys, ShortcutDescriptions, aMenuItem, MenuString);
                }
            }
            else if (parentComponent is ToolStripMenuItem)
            {
                string MenuString = parentMenuString + ((ToolStripMenuItem)parentComponent).Text;
                if (((ToolStripMenuItem)parentComponent).ShortcutKeys != Keys.None)
                {
                    ShortcutKeys.Add(((ToolStripMenuItem)parentComponent).ShortcutKeys.ToString());
                    ShortcutDescriptions.Add(MenuString);
                }
                foreach (ToolStripItem aMenuItem in ((ToolStripMenuItem)parentComponent).DropDownItems)
                {
                    addShortcutsInList(ShortcutKeys, ShortcutDescriptions, aMenuItem, MenuString + " - ");
                }
            }
            else if (parentComponent is ToolStripSeparator)
            {
                // nothing to do
            }
            else
            {
                foreach (Control aControl in ((Control)parentComponent).Controls)
                {
                    addShortcutsInList(ShortcutKeys, ShortcutDescriptions, aControl, "");
                }
            }
        }

        #endregion

        //*****************************************************************
        #region methods to read and write customization file
        //*****************************************************************

        // load the settings from file
        internal void loadCustomizationFile(string CustomizationFile, bool optionalSavePreviousChanges)
        {
            lastCustomizationFile = "";
            if (optionalSavePreviousChanges)
            {
                saveIfChangedAndConfirmed();
            }
            ArrayList CustomizationFileRows = new ArrayList();

            string line;
            int lineNo = 1;

            if (System.IO.File.Exists(CustomizationFile))
            {
                PropertyTableContainsComponentSettings = false;

                try
                {
                    // if file contains a BOM, StreamReader uses that encoding, else codepage configured in system is used (which is behaviour of QIC upt to 4.37)
                    System.IO.StreamReader StreamIn =
                      new System.IO.StreamReader(CustomizationFile, System.Text.Encoding.GetEncoding(0));
                    line = StreamIn.ReadLine();
                    while (line != null)
                    {
                        // analyze a single line of the file 
                        analyzeCustomizationFileLine(CustomizationFile, line, lineNo, CustomizationFileRows);
                        line = StreamIn.ReadLine();
                        lineNo++;
                    }
                    StreamIn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error when reading configuration file " + CustomizationFile + ", line "
                      + lineNo + "\n" + ex.Message);
                }
            }
            else
            {
                throw new Exception("Error: configuration file " + CustomizationFile + "not found.");
            }
            // successfully terminated, save name of file
            lastCustomizationFile = CustomizationFile;
        }

        // analyze one line in configuration file and extract configuration item
        private void analyzeCustomizationFileLine(string CustomizationFile, string line,
          int lineNo, ArrayList CustomizationFileRows)
        {
            string firstChar;
            string propertyTableKey;
            string propertyValueString;
            string ComponentFullName;
            string PropertyName;
            enumProperty propertyIndex;
            object PropertyObject;

            int RGB_Value = 0;
            int IndexColon;
            int IndexEqual;
            int IndexSemicolon;

            if (line.Length > 0)
            {
                firstChar = line.Substring(0, 1);

                if (firstChar.Equals(";"))
                {
                    // comment, nothing to do
                }
                else
                {
                    IndexEqual = line.IndexOf("=");
                    if (IndexEqual < 0)
                    {
                        throw new Exception("Content error in " + CustomizationFile + ", Zeile " + lineNo);
                    }

                    propertyTableKey = line.Substring(0, IndexEqual);
                    IndexColon = propertyTableKey.IndexOf(":");
                    if (IndexColon < 0)
                    {
                        throw new Exception("Content error in " + CustomizationFile + ", Zeile " + lineNo);
                    }
                    ComponentFullName = propertyTableKey.Substring(0, IndexColon);
                    PropertyName = propertyTableKey.Substring(IndexColon + 1);
                    if (PropertyName.Equals(PropertyNameZoomOld))
                    {
                        // get scaling factor and add in property table
                        float zoomFactor = float.Parse(line.Substring(IndexEqual + 1));
                        // a value of 1f in old zoom factor is considered as "not set"
                        // see also comment at definition of PropertyNameZoomOld
                        if (zoomFactor != 1f)
                        {
                            if (PropertyTable.ContainsKey(propertyTableKey))
                            {
                                ((PropertyPair)PropertyTable[propertyTableKey]).Customized = zoomFactor;
                            }
                            else
                            {
                                PropertyTable.Add(propertyTableKey, new PropertyPair(null, zoomFactor));
                            }
                        }
                    }
                    else if (PropertyName.Equals(PropertyNameZoom))
                    {
                        // get scaling factor and add in property table
                        float zoomFactor = float.Parse(line.Substring(IndexEqual + 1));
                        if (PropertyTable.ContainsKey(propertyTableKey))
                        {
                            ((PropertyPair)PropertyTable[propertyTableKey]).Customized = zoomFactor;
                        }
                        else
                        {
                            PropertyTable.Add(propertyTableKey, new PropertyPair(null, zoomFactor));
                        }
                    }
                    else
                    {
                        PropertyTableContainsComponentSettings = true;

                        propertyIndex = getPropertyIndex(PropertyName);

                        propertyValueString = line.Substring(IndexEqual + 1);
                        List<string> PropertyValueParts = new List<string>();
                        // separate value string 
                        IndexSemicolon = propertyValueString.IndexOf(";");
                        while (IndexSemicolon > 0)
                        {
                            PropertyValueParts.Add(propertyValueString.Substring(0, IndexSemicolon));
                            propertyValueString = propertyValueString.Substring(IndexSemicolon + 1).Trim();
                            IndexSemicolon = propertyValueString.IndexOf(";");
                        }
                        // add last part
                        PropertyValueParts.Add(propertyValueString.Trim());

                        // analyse the value
                        try
                        {
                            // colors
                            if (propertyIndex == enumProperty.BackColor ||
                                propertyIndex == enumProperty.ForeColor)
                            {
                                RGB_Value = int.Parse(PropertyValueParts[0]);
                                PropertyObject = Color.FromArgb(RGB_Value);
                            }
                            // font
                            else if (propertyIndex == enumProperty.Font)
                            {
                                FontStyle theStyle = FontStyle.Regular;
                                if (PropertyValueParts[2].Contains("Bold")) theStyle = theStyle | FontStyle.Bold;
                                if (PropertyValueParts[2].Contains("Italic")) theStyle = theStyle | FontStyle.Italic;
                                if (PropertyValueParts[2].Contains("Strikeout")) theStyle = theStyle | FontStyle.Strikeout;
                                if (PropertyValueParts[2].Contains("Underline")) theStyle = theStyle | FontStyle.Underline;
                                Font theFont = new Font(PropertyValueParts[0], (float)double.Parse(PropertyValueParts[1]), theStyle);
                                PropertyObject = theFont;
                            }
                            // integer values
                            else if (propertyIndex == enumProperty.Left ||
                                     propertyIndex == enumProperty.Top ||
                                     propertyIndex == enumProperty.Width ||
                                     propertyIndex == enumProperty.Height ||
                                     propertyIndex == enumProperty.TabIndex)
                            {
                                PropertyObject = int.Parse(propertyValueString);
                            }
                            // strings
                            else if (propertyIndex == enumProperty.Text ||
                                     propertyIndex == enumProperty.BackgroundImage)
                            {
                                PropertyObject = propertyValueString;
                            }
                            // boolean
                            else if (propertyIndex == enumProperty.AutoSize)
                            {
                                if (propertyValueString.Equals("True"))
                                {
                                    PropertyObject = true;
                                }
                                else if (propertyValueString.Equals("False"))
                                {
                                    PropertyObject = false;
                                }
                                else
                                {
                                    throw new Exception("allowed are \"True\" and \"False\"");
                                }
                            }
                            else if (propertyIndex == enumProperty.Shortcut)
                            {
                                PropertyObject = getKeysFromString(propertyValueString);
                            }
                            // unknown type
                            else
                            {
                                throw new Exception("Internal error");
                            }
                            if (PropertyTable.ContainsKey(propertyTableKey))
                            {
                                ((PropertyPair)PropertyTable[propertyTableKey]).Customized = PropertyObject;
                            }
                            else
                            {
                                PropertyTable.Add(propertyTableKey, new PropertyPair(null, PropertyObject));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error in configuration file. \""
                              + propertyValueString + "\" is no valid value for \""
                              + PropertyName + "\"\n"
                              + ex.Message);
                        }
                    }
                }
            }
        }

        // write the settings into file with asking for file name
        internal void writeCustomizationFile()
        {
            SaveFileDialog saveCustomizationFileDialog = new SaveFileDialog();
            saveCustomizationFileDialog.InitialDirectory = lastCustomizationFile;
            saveCustomizationFileDialog.DefaultExt = "ini";
            saveCustomizationFileDialog.Title = Customizer.getText(Texts.I_saveSettingsFile);
            saveCustomizationFileDialog.Filter = Customizer.getText(Texts.I_initFileTypesAllFiles);
            saveCustomizationFileDialog.RestoreDirectory = true;

            if (saveCustomizationFileDialog.ShowDialog() == DialogResult.OK)
            {
                writeCustomizationFile(saveCustomizationFileDialog.FileName);
            }
        }

        // write the settings into file
        internal void writeCustomizationFile(string CustomizationFile)
        {
            System.IO.StreamWriter StreamOut = null;

            try
            {
                StreamOut = new System.IO.StreamWriter(CustomizationFile, false, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception("Error writing " + CustomizationFile + "\n" + ex.Message);
            }

            StreamOut.WriteLine("; " + FileHeaderLine);
            StreamOut.WriteLine("; ---------------------------------------------------------------------------------------");
            StreamOut.WriteLine("; Alle Einträge werden über das Programm gepflegt. Manuelle Änderungen auf eigene Gefahr.");
            StreamOut.WriteLine("; All entries are maintained by the program. Manual changes at Your own risk.");
            StreamOut.WriteLine(";");

            // create sorted list of keys
            ArrayList SortedKeys = new ArrayList(PropertyTable.Keys);
            SortedKeys.Sort();

            // write the values sorted by key
            foreach (string Key in SortedKeys)
            {
                object PropertyObject = ((PropertyPair)PropertyTable[Key]).Customized;
                if (PropertyObject != null)
                {
                    if (PropertyObject is Color)
                    {
                        Color theColor = (Color)PropertyObject;
                        StreamOut.WriteLine(Key + "=" + theColor.ToArgb() + " ;" + theColor.ToString());
                    }
                    else if (PropertyObject is Font)
                    {
                        Font theFont = (Font)PropertyObject;
                        StreamOut.WriteLine(Key + "=" + theFont.FontFamily.Name + "; "
                          + theFont.Size.ToString() + "; " + theFont.Style.ToString());
                    }
                    else if (Key.EndsWith(":" + PropertyNameZoom))
                    {
                        // write entry only, if it differs from general zoom factor
                        if (generalZoomFactor != (float)PropertyObject)
                        {
                            StreamOut.WriteLine(Key + "=" + PropertyObject.ToString());
                        }
                    }
                    else
                    {
                        StreamOut.WriteLine(Key + "=" + PropertyObject.ToString());
                    }
                }
            }

            StreamOut.Close();
            // successfully terminated, save name of file
            lastCustomizationFile = CustomizationFile;
            customizedSettingChanged = false;
            return;
        }
        #endregion

        //*****************************************************************
        #region methods to get form component from full name and vice versa
        //*****************************************************************

        // get component from full name
        private static Component getComponentFromFullName(Form theForm, string ComponentFullName)
        {
            int IndexDot;
            string firstPart = "";
            string secondPart = ComponentFullName;
            Component ChangeableComponent = null;

            do
            {
                IndexDot = secondPart.IndexOf(".");
                if (IndexDot > 0)
                {
                    // separate the string
                    firstPart = secondPart.Substring(0, IndexDot);
                    secondPart = secondPart.Substring(IndexDot + 1);
                }
                else
                {
                    // last part found
                    firstPart = secondPart;
                    secondPart = "";
                }

                // when adding new types: search for add-new-type-here to find 
                // all other locations where changes are necessary!!!
                if (ChangeableComponent == null)
                {
                    if (firstPart.Equals(theForm.Name))
                    {
                        ChangeableComponent = theForm;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (ChangeableComponent is TabControl)
                {
                    int indexPage = int.Parse(firstPart);
                    ChangeableComponent = ((TabControl)ChangeableComponent).TabPages[indexPage];
                }
                else if (ChangeableComponent is SplitContainer)
                {
                    SplitContainer theSplitContainer = (SplitContainer)ChangeableComponent;
                    if (firstPart.Equals("1"))
                    {
                        ChangeableComponent = theSplitContainer.Panel1;
                    }
                    else
                    {
                        ChangeableComponent = theSplitContainer.Panel2;
                    }
                }
                else if (ChangeableComponent is MenuStrip)
                {
                    MenuStrip theMenuStrip = (MenuStrip)ChangeableComponent;
                    ChangeableComponent = theMenuStrip.Items[firstPart];
                }
                else if (ChangeableComponent is ToolStripMenuItem)
                {
                    ToolStripMenuItem theToolStripMenuItem = (ToolStripMenuItem)ChangeableComponent;
                    ChangeableComponent = theToolStripMenuItem.DropDownItems[firstPart];
                }
                else if (ChangeableComponent is ToolStripDropDownButton)
                {
                    ToolStripDropDownButton theToolStripDropDownButton = (ToolStripDropDownButton)ChangeableComponent;
                    ChangeableComponent = theToolStripDropDownButton.DropDownItems[firstPart];
                }
                else
                {
                    ChangeableComponent = ((Control)ChangeableComponent).Controls[firstPart];
                }
                IndexDot = secondPart.IndexOf(".");
            }
            while (!secondPart.Equals(""));

            return ChangeableComponent;
        }

        // get full name of form component (with all parents)
        internal static string getFullNameOfComponent(Component theComponent)
        {
            string theName = "";
            if (theComponent is Control)
            {
                theName = getNameOfControl((Control)theComponent);
                Control tempControl = (Control)theComponent;
                while (tempControl.Parent != null)
                {
                    tempControl = tempControl.Parent;
                    theName = getNameOfControl(tempControl) + "." + theName;
                }
            }
            else if (theComponent is ToolStripItem)
            {
                ToolStripItem tempToolStripItem = (ToolStripItem)theComponent;
                theName = tempToolStripItem.Name;
                while (tempToolStripItem.OwnerItem != null)
                {
                    tempToolStripItem = tempToolStripItem.OwnerItem;
                    theName = tempToolStripItem.Name + "." + theName;
                }
                Control theParentControl = tempToolStripItem.GetCurrentParent();
                theName = getFullNameOfComponent(theParentControl) + "." + theName;
            }
            return removeSpecificLeadingPartsControlFullNamePrefixDollar(theName);
        }

        // remove specific leading parts of control full name
        // the controls may move to different panels in form, so ignore this variable leading part
        private static string removeSpecificLeadingPartsControlFullNamePrefixDollar(string theName)
        {
            string nameModified = string.Copy(theName);
            for (int ii = 0; ii < leadingControlNamePartsToIgnore.Length; ii++)
            {
                if (nameModified.StartsWith(leadingControlNamePartsToIgnore[ii]))
                {
                    // use $ as prefix so that these controls are sorted before those with path starting with form name
                    nameModified = "$" + nameModified.Substring(leadingControlNamePartsToIgnore[ii].Length);
                    break;
                }
            }
            for (int ii = 0; ii < leadingControlNamePartsPrefixDollar.Length; ii++)
            {
                if (nameModified.StartsWith(leadingControlNamePartsPrefixDollar[ii]))
                {
                    // use $ as prefix so that these controls are sorted before those with path starting with form name
                    nameModified = "$" + nameModified;
                    break;
                }
            }
            return nameModified;
        }

        // get name of control, considering special controls like tab page and panel
        private static string getNameOfControl(Control theControl)
        {
            if (theControl is TabPage)
            {
                TabControl theTabControl = (TabControl)theControl.Parent;
                for (int ii = 0; ii < theTabControl.TabPages.Count; ii++)
                {
                    if (theControl == theTabControl.TabPages[ii])
                    {
                        return ii.ToString();
                    }
                }
                // should never come to here
                throw new Exception("Internal error");
            }
            else if (theControl is SplitterPanel)
            {
                SplitContainer theSplitContainer = (SplitContainer)theControl.Parent;
                if (theSplitContainer.Panel1.Equals(theControl))
                {
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            else
            {
                return theControl.Name;
            }
        }
        #endregion

        //*****************************************************************
        #region utility methods
        //*****************************************************************

        // get property index from property name
        private enumProperty getPropertyIndex(string PropertyName)
        {
            for (int ii = 0; ii < PropertyNames.GetLength(0); ii++)
            {
                if (PropertyNames[ii].Equals(PropertyName))
                {
                    return (enumProperty)ii;
                }
            }
            throw new Exception("Internal error");
        }

        // gets the property of given form component
        internal object getProperty(Component givenComponent, enumProperty propertyIndex)
        {
            if (givenComponent is Control)
            {
                Control givenControl = (Control)givenComponent;
                switch (propertyIndex)
                {
                    case enumProperty.BackColor:
                        return givenControl.BackColor;
                    case enumProperty.ForeColor:
                        return givenControl.ForeColor;
                    case enumProperty.Font:
                        return givenControl.Font;
                    case enumProperty.Left:
                        return givenControl.Left;
                    case enumProperty.Top:
                        return givenControl.Top;
                    case enumProperty.Width:
                        return givenControl.Width;
                    case enumProperty.Height:
                        return givenControl.Height;
                    case enumProperty.TabIndex:
                        return givenControl.TabIndex;
                    case enumProperty.Text:
                        return givenControl.Text;
                    case enumProperty.BackgroundImage:
                        return givenControl.BackgroundImage;
                    case enumProperty.AutoSize:
                        if (givenControl is CheckBox)
                            return ((CheckBox)givenControl).AutoSize;
                        else if (givenControl is DomainUpDown)
                            return ((DomainUpDown)givenControl).AutoSize;
                        else if (givenControl is Label)
                            return ((Label)givenControl).AutoSize;
                        else if (givenControl is LinkLabel)
                            return ((LinkLabel)givenControl).AutoSize;
                        else if (givenControl is MaskedTextBox)
                            return ((MaskedTextBox)givenControl).AutoSize;
                        else if (givenControl is NumericUpDown)
                            return ((NumericUpDown)givenControl).AutoSize;
                        else if (givenControl is RadioButton)
                            return ((RadioButton)givenControl).AutoSize;
                        else if (givenControl is TextBox)
                            return ((TextBox)givenControl).AutoSize;
                        else if (givenControl is TrackBar)
                            return ((TrackBar)givenControl).AutoSize;
                        else if (givenControl is Button)
                            return ((Button)givenControl).AutoSize;
                        else if (givenControl is CheckedListBox)
                            return ((CheckedListBox)givenControl).AutoSize;
                        else if (givenControl is FlowLayoutPanel)
                            return ((FlowLayoutPanel)givenControl).AutoSize;
                        else if (givenControl is Form)
                            return ((Form)givenControl).AutoSize;
                        else if (givenControl is GroupBox)
                            return ((GroupBox)givenControl).AutoSize;
                        else if (givenControl is Panel)
                            return ((Panel)givenControl).AutoSize;
                        else if (givenControl is TableLayoutPanel)
                            return ((TableLayoutPanel)givenControl).AutoSize;
                        else
                            return false;
                    case enumProperty.Shortcut:
                        foreach (string aShortcutKey in ShortcutsForHandler.Keys)
                        {
                            if (ShortcutsForHandler[aShortcutKey].Equals(givenControl))
                            {
                                int index = aShortcutKey.IndexOf(".");
                                return getKeysFromString(aShortcutKey.Substring(index + 1));
                            }
                        }
                        return Keys.None;
                    default:
                        throw new Exception("Internal error");
                }
            }
            else if (givenComponent is ToolStripItem)
            {
                // when adding new types: search for add-new-type-here to find 
                // all other locations where changes are necessary!!!
                ToolStripItem givenToolStripItem = (ToolStripItem)givenComponent;
                switch (propertyIndex)
                {
                    case enumProperty.BackColor:
                        return givenToolStripItem.BackColor;
                    case enumProperty.ForeColor:
                        return givenToolStripItem.ForeColor;
                    case enumProperty.Font:
                        return givenToolStripItem.Font;
                    case enumProperty.Left:
                        // not defined for ToolStripItem
                        return 0;
                    case enumProperty.Top:
                        // not defined for ToolStripItem
                        return 0;
                    case enumProperty.Width:
                        return givenToolStripItem.Width;
                    case enumProperty.Height:
                        return givenToolStripItem.Height;
                    case enumProperty.TabIndex:
                        // not defined for ToolStripItem
                        return 0;
                    case enumProperty.Text:
                        return givenToolStripItem.Text;
                    case enumProperty.BackgroundImage:
                        return givenToolStripItem.BackgroundImage;
                    case enumProperty.AutoSize:
                        return givenToolStripItem.AutoSize;
                    case enumProperty.Shortcut:
                        if (givenToolStripItem is ToolStripMenuItem)
                        {
                            return ((ToolStripMenuItem)givenToolStripItem).ShortcutKeys;
                        }
                        else
                        {
                            return null;
                        }
                    default:
                        throw new Exception("Internal error");
                }
            }
            else
            {
                throw new Exception("Internal error");
            }
        }

        // sets the property of given control
        private void setProperty(Component givenComponent, enumProperty propertyIndex, object PropertyValue)
        {
            if (givenComponent is Control)
            {
                Control givenControl = (Control)givenComponent;
                switch (propertyIndex)
                {
                    case enumProperty.BackColor:
                        if (givenControl is DataGridView)
                        {
                            ((DataGridView)givenControl).BackgroundColor = (Color)PropertyValue;
                        }
                        else
                        {
                            givenControl.BackColor = (Color)PropertyValue;
                        }
                        break;
                    case enumProperty.ForeColor:
                        givenControl.ForeColor = (Color)PropertyValue;
                        break;
                    case enumProperty.Font:
                        givenControl.Font = (Font)PropertyValue;
                        break;
                    case enumProperty.Left:
                        givenControl.Left = (int)PropertyValue;
                        break;
                    case enumProperty.Top:
                        givenControl.Top = (int)PropertyValue;
                        break;
                    case enumProperty.Width:
                        // deactivate AutoSize for menu strip and status strip to ensure that control changes size
                        // for other controls it can have the effect, that text is truncated
                        if (givenControl is MenuStrip || givenControl is StatusStrip)
                        {
                            givenControl.AutoSize = false;
                        }
                        givenControl.Width = (int)PropertyValue;
                        break;
                    case enumProperty.Height:
                        // deactivate AutoSize for menu strip and status strip to ensure that control changes size
                        // for other controls it can have the effect, that text is truncated
                        if (givenControl is MenuStrip || givenControl is StatusStrip)
                        {
                            givenControl.AutoSize = false;
                        }
                        givenControl.Height = (int)PropertyValue;
                        break;
                    case enumProperty.TabIndex:
                        givenControl.TabIndex = (int)PropertyValue;
                        break;
                    case enumProperty.Text:
                        givenControl.Text = (string)PropertyValue;
                        break;
                    case enumProperty.BackgroundImage:
                        if (PropertyValue == null)
                        {
                            givenControl.BackgroundImage = null;
                        }
                        else
                        {
                            try
                            {
                                givenControl.BackgroundImage = System.Drawing.Image.FromFile((string)PropertyValue);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(Customizer.getText(Texts.E_loadingConfiguration)
                                    + "\n" + Customizer.getText(Texts.I_imageFileNotExist)
                                    + "\n" + ex.Message);
                            }
                        }
                        break;
                    case enumProperty.AutoSize:
                        if (givenControl is CheckBox)
                            ((CheckBox)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is DomainUpDown)
                            ((DomainUpDown)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is Label)
                            ((Label)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is LinkLabel)
                            ((LinkLabel)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is MaskedTextBox)
                            ((MaskedTextBox)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is NumericUpDown)
                            ((NumericUpDown)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is RadioButton)
                            ((RadioButton)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is TextBox)
                            ((TextBox)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is TrackBar)
                            ((TrackBar)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is Button)
                            ((Button)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is CheckedListBox)
                            ((CheckedListBox)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is FlowLayoutPanel)
                            ((FlowLayoutPanel)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is Form)
                            ((Form)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is GroupBox)
                            ((GroupBox)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is Panel)
                            ((Panel)givenControl).AutoSize = (bool)PropertyValue;
                        else if (givenControl is TableLayoutPanel)
                            ((TableLayoutPanel)givenControl).AutoSize = (bool)PropertyValue;
                        else
                            throw new Exception("Internal error");
                        break;
                    case enumProperty.Shortcut:
                        string ShortcutKeyString = "";
                        Keys theShortcut = (Keys)PropertyValue;
                        if (!theShortcut.Equals(Keys.None))
                        {
                            Control ParentControl = givenControl;
                            while (ParentControl.Parent != null)
                            {
                                ParentControl = ParentControl.Parent;
                            }
                            ShortcutKeyString = ParentControl.Name + "." + theShortcut.ToString();
                        }
                        addOverwriteShortcutInTable(ShortcutKeyString, givenControl);
                        break;
                    default:
                        throw new Exception("Internal error");
                }
            }
            // when adding new types: search for add-new-type-here to find 
            // all other locations where changes are necessary!!!
            else if (givenComponent is ToolStripItem)
            {
                ToolStripItem givenToolStripItem = (ToolStripItem)givenComponent;
                switch (propertyIndex)
                {
                    case enumProperty.BackColor:
                        givenToolStripItem.BackColor = (Color)PropertyValue;
                        break;
                    case enumProperty.ForeColor:
                        givenToolStripItem.ForeColor = (Color)PropertyValue;
                        break;
                    case enumProperty.Font:
                        givenToolStripItem.Font = (Font)PropertyValue;
                        break;
                    case enumProperty.Left:
                        throw new Exception("Internal error: no property \"Left\"");
                    case enumProperty.Top:
                        throw new Exception("Internal error: no property \"Top\"");
                    case enumProperty.Width:
                        givenToolStripItem.Width = (int)PropertyValue;
                        break;
                    case enumProperty.Height:
                        givenToolStripItem.Height = (int)PropertyValue;
                        break;
                    case enumProperty.TabIndex:
                        throw new Exception("Internal error: no property \"TabIndex\"");
                    case enumProperty.Text:
                        givenToolStripItem.Text = (string)PropertyValue;
                        break;
                    case enumProperty.BackgroundImage:
                        givenToolStripItem.BackgroundImage = System.Drawing.Image.FromFile((string)PropertyValue);
                        break;
                    case enumProperty.AutoSize:
                        givenToolStripItem.AutoSize = (bool)PropertyValue;
                        break;
                    case enumProperty.Shortcut:
                        if (givenToolStripItem is ToolStripMenuItem)
                        {
                            ((ToolStripMenuItem)givenToolStripItem).ShortcutKeys = (Keys)PropertyValue;
                        }
                        else
                        {
                            throw new Exception("Internal error");
                        }
                        break;
                    default:
                        throw new Exception("Internal error");
                }
            }
            else if (givenComponent is ToolStripItem)
            {
                ToolStripItem givenToolStripItem = (ToolStripItem)givenComponent;
                switch (propertyIndex)
                {
                    case enumProperty.BackColor:
                        givenToolStripItem.BackColor = (Color)PropertyValue;
                        break;
                    case enumProperty.ForeColor:
                        givenToolStripItem.ForeColor = (Color)PropertyValue;
                        break;
                    case enumProperty.Font:
                        givenToolStripItem.Font = (Font)PropertyValue;
                        break;
                    case enumProperty.Left:
                        throw new Exception("Internal error: no property \"Left\"");
                    case enumProperty.Top:
                        throw new Exception("Internal error: no property \"Top\"");
                    case enumProperty.Width:
                        givenToolStripItem.Width = (int)PropertyValue;
                        break;
                    case enumProperty.Height:
                        givenToolStripItem.Height = (int)PropertyValue;
                        break;
                    case enumProperty.TabIndex:
                        throw new Exception("Internal error: no property \"TabIndex\"");
                    case enumProperty.Text:
                        givenToolStripItem.Text = (string)PropertyValue;
                        break;
                    case enumProperty.BackgroundImage:
                        givenToolStripItem.BackgroundImage = System.Drawing.Image.FromFile((string)PropertyValue);
                        break;
                    case enumProperty.AutoSize:
                        givenToolStripItem.AutoSize = (bool)PropertyValue;
                        break;
                    case enumProperty.Shortcut:
                        if (givenToolStripItem is ToolStripMenuItem)
                        {
                            ((ToolStripMenuItem)givenToolStripItem).ShortcutKeys = (Keys)PropertyValue;
                        }
                        else
                        {
                            throw new Exception("Internal error");
                        }
                        break;
                    default:
                        throw new Exception("Internal error");
                }
            }
        }

        // set property of form component, given as object, and save settings in property table
        internal void setPropertyByObjectAndSaveSettings(Component ChangeableComponent,
          enumProperty propertyIndex, object newProperty, float actualZoomFactor)
        {
            string ComponentFullName = getFullNameOfComponent(ChangeableComponent);
            string propertyTableKey = ComponentFullName + ":" + PropertyNames[(int)propertyIndex];
            if (PropertyTable.ContainsKey(propertyTableKey))
            {
                // property already in list, update customized value and if not yet done: set original
                ((PropertyPair)PropertyTable[propertyTableKey]).Customized = newProperty;
                if (((PropertyPair)PropertyTable[propertyTableKey]).Original == null)
                {
                    ((PropertyPair)PropertyTable[propertyTableKey]).Original = getProperty(ChangeableComponent, propertyIndex);
                }
            }
            else
            {
                // property not in list, add it with original and customized value
                PropertyTable.Add(propertyTableKey,
                  new PropertyPair(getProperty(ChangeableComponent, propertyIndex), newProperty));
            }
            setProperty(ChangeableComponent, propertyIndex, newProperty);
            if (ChangeableComponent is Control)
            {
                addOrUpdateZoomBasisData((Control)ChangeableComponent, actualZoomFactor);
            }

            // necessary to force owner draw of control and that is necessary to change color
            if (ChangeableComponent is TabPage)
            {
                ((Control)ChangeableComponent).Parent.Hide();
                ((Control)ChangeableComponent).Refresh();
                ((Control)ChangeableComponent).Parent.Show();
            }
            customizedSettingChanged = true;
        }

        // reset property of component, given as Component-object
        internal void resetPropertyByObject(Component ChangeableComponent, enumProperty propertyIndex)
        {
            string ComponentFullName = getFullNameOfComponent(ChangeableComponent);
            string propertyTableKey = ComponentFullName + ":" + PropertyNames[(int)propertyIndex];
            if (PropertyTable.ContainsKey(propertyTableKey))
            {
                object originalProperty = ((PropertyPair)PropertyTable[propertyTableKey]).Original;
                setProperty(ChangeableComponent, propertyIndex, originalProperty);
                ((PropertyPair)PropertyTable[propertyTableKey]).Customized = null;
            }

            // necessary to force owner draw of control and that is necessary to change color
            if (ChangeableComponent is TabPage)
            {
                ((Control)ChangeableComponent).Parent.Hide();
                ((Control)ChangeableComponent).Refresh();
                ((Control)ChangeableComponent).Parent.Show();
            }
            customizedSettingChanged = true;
        }

        // return keys from string
        internal static Keys getKeysFromString(string KeyString)
        {
            foreach (Keys Key in Enum.GetValues(typeof(Shortcut)))
            {
                if (Key.ToString().Equals(KeyString))
                {
                    return Key;
                }
            }
            MessageBox.Show(Customizer.getText(Texts.W_noValidShortcut, KeyString));
            return Keys.None;
        }

        // get and set general zoom factor
        internal static float getGeneralZoomFactor()
        {
            return generalZoomFactor;
        }
        internal static void setGeneralZoomFactor(float value)
        {
            generalZoomFactor = value;
        }

        // get zoomed font
        // the width of a text does not change proportional to font size
        // font size is determined to ensure, that text fits into boundaries
        internal static Font getZoomedFont(Font usedFont, float initialFontSize, float zoomFactor)
        {
            int maxWidth;
            int newFontSize;
            Font newFont;
            SizeF newSize;
            Font initialFont = new Font(usedFont.FontFamily, initialFontSize, usedFont.Style);

            if (zoomFactor == 1f)
            {
                return initialFont;
            }

            //string key = oldFont.ToString() + zoomFactor.ToString();
            string key = usedFont.Name + initialFontSize.ToString() + " " + zoomFactor.ToString();
            if (NewFontSizesForZoom.ContainsKey(key))
            {
                return new Font(usedFont.FontFamily, NewFontSizesForZoom[key], usedFont.Style);
            }
            else
            {
                SizeF oldSize = TextRenderer.MeasureText(fontSizeTest, initialFont);
                maxWidth = (int)(oldSize.Width * zoomFactor);
                // start with font size proportional to zoom factor +1 as tolerance
                // note that newFontSize is decremented at begin of loop
                newFontSize = (int)(initialFontSize * zoomFactor) + 2;
                do
                {
                    newFontSize--;
                    newFont = new Font(usedFont.FontFamily, newFontSize, usedFont.Style);
                    newSize = TextRenderer.MeasureText(fontSizeTest, newFont);
                }
                while (newSize.Width > maxWidth);
                NewFontSizesForZoom.Add(key, newFontSize);
                return newFont;
            }
        }
        #endregion
    }
}
