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

using QuickImageComment;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FormCustomization
{
    public class Interface
    {
        private Customizer theCustomizer;

        // constructor
        public Interface(Form theForm, string CustomizationFile, string FileHeaderLine, string HelpUrl, string HelpTopic)
        {
            theCustomizer = new Customizer(FileHeaderLine, HelpUrl, HelpTopic, new SortedList<string, string>());
            if (!CustomizationFile.Equals(""))
            {
                theCustomizer.loadCustomizationFile(CustomizationFile, true);
            }
            // when initiating Customization Interface, nothing can have been zoomed before,
            // so no need to call setFormToCustomizedValuesZoomOnInitial which removes zoom basis data
            setFormToCustomizedValuesZoomIfChangedNoHideDuringModification(theForm);

            // add generic key event handler
            theForm.KeyDown += new System.Windows.Forms.KeyEventHandler(theCustomizer.Form_KeyDown);
        }

        public Interface(Form theForm, string CustomizationFile, string FileHeaderLine, string HelpUrl, string HelpTopic,
            SortedList<string, string> givenTranslations, string[] leadingControlNamePartsToIgnore,
            string[] leadingControlNamePartsPrefixDollar)
        {
            theCustomizer = new Customizer(FileHeaderLine, HelpUrl, HelpTopic, givenTranslations);
            if (!CustomizationFile.Equals(""))
            {
                theCustomizer.loadCustomizationFile(CustomizationFile, true);
            }
            Customizer.leadingControlNamePartsToIgnore = leadingControlNamePartsToIgnore;
            Customizer.leadingControlNamePartsPrefixDollar = leadingControlNamePartsPrefixDollar;

            // when initiating Customization Interface, nothing can have been zoomed before,
            // so no need to call setFormToCustomizedValuesZoomOnInitial which removes zoom basis data
            setFormToCustomizedValuesZoomIfChangedNoHideDuringModification(theForm);

            // add generic key event handler
            theForm.KeyDown += new System.Windows.Forms.KeyEventHandler(theCustomizer.Form_KeyDown);
        }

        // set list of translations
        public void setTranslations(SortedList<string, string> givenTranslations)
        {
            Customizer.setTranslations(givenTranslations);
        }
        // return the used translations
        public ArrayList getUsedTranslations()
        {
            return Customizer.getUsedTranslations();
        }
        // return not translated texts
        public ArrayList getNotTranslatedTexts()
        {
            return Customizer.getNotTranslatedTexts();
        }
        // get the name of last loaded or saved customization settings file
        public string getLastCustomizationFile()
        {
            return theCustomizer.getLastCustomizationFile();
        }

        // clear name of last loaded or saved customization settings file
        public void clearLastCustomizationFile()
        {
            theCustomizer.clearLastCustomizationFile();
        }

        // clear flag indicating that customized settings were changed
        public void clearCustomizedSettingsChanged()
        {
            theCustomizer.clearCustomizedSettingsChanged();
        }

        // set properties of all form components based on original settings
        // remove zoom basis data and zoom
        // when same form is created again, old zoom data are still in Customizer,
        // so they are deleted in order not to zoom based on wrong data
        // to be called before .Show (as controls are not hidden during modification)
        public void setFormToCustomizedValuesZoomInitial(Form theForm)
        {
            theCustomizer.setAllComponentsZoomInitial(Customizer.enumSetTo.Customized, theForm);
        }

        // set properties of all form components based on original settings
        // zoom only if zoom factor changed
        public void setFormToCustomizedValuesZoomIfChanged(Form theForm)
        {
            theCustomizer.setAllComponentsZoomIfChanged(Customizer.enumSetTo.Customized, theForm);
        }

        // set properties of all form components based on original settings
        // zoom only if zoom factor changed; no hide of controls during modification
        public void setFormToCustomizedValuesZoomIfChangedNoHideDuringModification(Form theForm)
        {
            theCustomizer.setAllComponentsZoomIfChangedNoHideDuringModfication(Customizer.enumSetTo.Customized, theForm);
        }

        // zoom controls including childs using target zoom factor (general or form specific)
        // applied zoom factor is used for update zoom basis data (if required)
        public void zoomControlsUsingTargetZoomFactor(Control ParentControl, Form ContainingForm)
        {
            theCustomizer.zoomControlsUsingTargetZoomFactor(ParentControl, ContainingForm);
        }

        // zoom controls including childs
        internal void zoomControls(Control ParentControl, float zoomFactor)
        {
            theCustomizer.zoomControls(ParentControl, zoomFactor);
        }

        // zoom tool strip (limited zooming)
        internal void zoomToolStrip(Control ParentControl, float zoomFactor)
        {
            theCustomizer.zoomToolStrip(ParentControl, zoomFactor);
        }

        // load the settings from file
        public void loadCustomizationFile(string CustomizationFile)
        {
            theCustomizer.loadCustomizationFile(CustomizationFile, true);
        }
        public void loadCustomizationFileNoOptionalSavePrevChanges(string CustomizationFile)
        {
            theCustomizer.loadCustomizationFile(CustomizationFile, false);
        }

        // write the settings into file
        public void writeCustomizationFile(string CustomizationFile)
        {
            theCustomizer.writeCustomizationFile(CustomizationFile);
        }

        // show form to change customization
        public void showFormCustomization(Form theForm)
        {
            FormCustomization theFormCustomization = new FormCustomization(theForm, theCustomizer);
            theFormCustomization.Show();
        }

        // get zoom factor of form
        public float getActualZoomFactor(Form theForm)
        {
            return theCustomizer.getActualZoomFactor(theForm);
        }

        // if settings are changed and saving is confirmed, save settings
        public void saveIfChangedAndConfirmed()
        {
            theCustomizer.saveIfChangedAndConfirmed();
        }

        // show list of keys
        public void showListOfKeys(Form theForm)
        {
            theCustomizer.showListOfKeys(theForm);
        }

        // reset the form
        // zoom only if zoomed before
        public void resetForm(Form theForm)
        {
            theCustomizer.setAllComponentsZoomIfChanged(Customizer.enumSetTo.Original, theForm);
        }

        // get and set general zoom factor
        public static float getGeneralZoomFactor()
        {
            return Customizer.getGeneralZoomFactor();
        }
        public static void setGeneralZoomFactor(float value)
        {
            Customizer.setGeneralZoomFactor(value);
        }

        // get zoom factor for font
        // the width of a text does not change proportional to font size
        // to ensure, that text fits into boundaries, zoom factor for font size is adjusted
        public static Font getZoomedFont(Font usedFont, float initialFontSize, float zoomFactor)
        {
            return Customizer.getZoomedFont(usedFont, initialFontSize, zoomFactor);
        }

        // fill the hashtable with zoom basis data of the control and its childs
        public void fillOrUpdateZoomBasisData(Control ParentControl, float actualZoomFactor)
        {
            theCustomizer.fillOrUpdateZoomBasisData(ParentControl, actualZoomFactor);
        }

        // remove entries in hashtable with zoom basis data of the control and its childs
        internal void removeZoomBasisData(Control ParentControl)
        {
            theCustomizer.removeZoomBasisData(ParentControl);
        }

        // can be used to check if all controls are scaled properly
        internal void checkFontSize(Control parent, float fontSize)
        {
            foreach (Control child in parent.Controls)
            {
                if (child.Font.Size != fontSize) Logger.log("# " + Customizer.getFullNameOfComponent(child).Replace("splitContainer", "SP") + " " + child.Font.Size.ToString()); // permanent use of Logger.log
                if (!child.Font.Name.Equals("Tahoma")) Logger.log("# " + Customizer.getFullNameOfComponent(child).Replace("splitContainer", "SP") + " " + child.Font.Name); // permanent use of Logger.log
                checkFontSize(child, fontSize);
            }
        }
    }
}
