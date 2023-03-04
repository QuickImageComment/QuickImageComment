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
                setFormToCustomizedValues(theForm);
            }
            // add generic key event handler
            theForm.KeyDown += new System.Windows.Forms.KeyEventHandler(theCustomizer.Form_KeyDown);
        }

        public Interface(Form theForm, string CustomizationFile, string FileHeaderLine, string HelpUrl, string HelpTopic,
            SortedList<string, string> givenTranslations)
        {
            theCustomizer = new Customizer(FileHeaderLine, HelpUrl, HelpTopic, givenTranslations);
            if (!CustomizationFile.Equals(""))
            {
                theCustomizer.loadCustomizationFile(CustomizationFile, true);
            }
            setFormToCustomizedValues(theForm);

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

        // zoom form and set properties of all form components to customized values
        public void setFormToCustomizedValues(Form theForm)
        {
            theCustomizer.setAllComponents(Customizer.enumSetTo.Customized, theForm);
        }

        // zoom form and set properties of all form components to original values
        public void setFormToOriginalValues(Form theForm)
        {
            theCustomizer.setAllComponents(Customizer.enumSetTo.Original, theForm);
        }

        // zoom controls (e.g. a user control)
        public void zoomControlsUsingGeneralZoomFactor(string prefix, Control ParentControl)
        {
            theCustomizer.zoomControlsUsingGeneralZoomFactor(ParentControl);
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
        public void resetForm(Form theForm)
        {
            theCustomizer.setAllComponents(Customizer.enumSetTo.Original, theForm);
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
    }
}
