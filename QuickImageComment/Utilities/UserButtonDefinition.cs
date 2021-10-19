namespace QuickImageComment
{
    public class UserButtonDefinition
    {
        public string text;
        public string tag;
        public string iconSpec;


        public UserButtonDefinition(
            string givenText,
            string givenTag,
            string givenIconSpec)
        {
            text = givenText;
            tag = givenTag;
            iconSpec = givenIconSpec;
        }

        public UserButtonDefinition(string DefinitionString)
        {
            int startIndex = 0;
            int endIndex = 0;
            text = "";
            tag = "";
            iconSpec = "";

            endIndex = DefinitionString.IndexOf("|", startIndex);
            text = DefinitionString.Substring(startIndex, endIndex - startIndex);

            startIndex = endIndex + 1;
            endIndex = DefinitionString.IndexOf("|", startIndex);
            tag = DefinitionString.Substring(startIndex, endIndex - startIndex);

            startIndex = endIndex + 1;
            iconSpec = DefinitionString.Substring(startIndex);
        }

        public override string ToString()
        {
            return text + "|" + tag + "|" + iconSpec;
        }
    }
}
