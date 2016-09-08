using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Script_Builder
{   
    class vorlage
    {
        List<textZeile> vorlageZeilen;
        string[] variableNames;
        string variableMarker;
        public vorlage(string[] vorlage, string[] captions, string variableMarker)
        {
            this.variableMarker = variableMarker;
            this.variableNames = captions;
            this.vorlageZeilen = analyse(vorlage);
        }

        public List<textZeile> analyse(string[] vorlage)
        {
            List<textZeile> zeilenList = new List<textZeile>();
            int ifBlock = 0;
            List<string> ifBlockList = new List<string>();
            Dictionary<int, string> ifCaseVariableList = new Dictionary<int,string>();
            for (int lineNo = 0; lineNo < vorlage.Length; lineNo++)
            {
                if (vorlage[lineNo].StartsWith("/IF "))
                {
                    if (ifBlock == 0)
                        ifBlockList = new List<string>();
                    ifBlockList.Add(vorlage[lineNo]);
                    ifCaseVariableList = lineVariableList(vorlage[lineNo], variableNames);
                    ifBlock++;
                }
                else if (vorlage[lineNo].StartsWith("/FI"))
                {
                    ifBlockList.Add(vorlage[lineNo]);
                    ifBlock--;
                    if (ifBlock == 0)
                        zeilenList.Add(new ifBlock(this, ifBlockList.ToArray(), ifCaseVariableList));
                }
                else if (ifBlock > 0)
                {
                    ifBlockList.Add(vorlage[lineNo]);
                }
                else
                {
                    if (!vorlage[lineNo].StartsWith("//"))
                    {
                        string thisLine = vorlage[lineNo];
                        if (thisLine.Contains(@" //"))
                        {
                            thisLine = thisLine.Substring(0, thisLine.IndexOf(@" //"));
                        }
                        Dictionary<int, string> thisLineVariableList = lineVariableList(thisLine, variableNames);
                        if (thisLineVariableList.Count > 0)
                            zeilenList.Add(new Variable(thisLine, thisLineVariableList));
                        else
                            zeilenList.Add(new textOnly(thisLine));
                    }
                }
            }
            return zeilenList;
        }

        private Dictionary<int, string> lineVariableList(string lineText,string[] captions)
        {
            Regex regex;
            string pattern;
            Dictionary<int,string> returnVariableList = new Dictionary<int,string>();
            for (int index = 0; index < captions.Length; index++)
            {
                pattern = variableMarker + captions[index] + @"(\[[0-9$]+[-+]?[0-9$]*,[0-9$]+[-+]?[0-9$]*\])?" + variableMarker; //@"\[([0-9$\-+]*),([0-9$\-+]*)\]"
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (regex.IsMatch(lineText))
                {
                    returnVariableList.Add(index, captions[index]);
                }
            }
            return returnVariableList;
        }

        public static string patternSearch(string textLine, string variableMarker, string variableName, string replaceWith)
        {
            string pattern = variableMarker + variableName + variableMarker;
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (regex.IsMatch(textLine))
            {
                textLine = textLine.Replace(pattern, replaceWith);
            }
            pattern = variableMarker + variableName + @"\[([0-9$]+[-+]?[0-9$]*),([0-9$]+[-+]?[0-9$]*)\]" + variableMarker; //@"\[([0-9$\-+]*),([0-9$\-+]*)\]"
            regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (regex.IsMatch(textLine))
            {
                List<string> patternHandled = new List<string>();
                MatchCollection result = regex.Matches(textLine);
                foreach (Match matching in result)
                {
                    string patternFound = matching.Groups[0].ToString();
                    if (!patternHandled.Contains(patternFound))
                    {
                        string posFrom = matching.Groups[1].ToString();
                        string posTo = matching.Groups[2].ToString();
                        posFrom = calcString(posFrom.Replace("$", (replaceWith.Length - 1).ToString()));
                        posTo = calcString(posTo.Replace("$", (replaceWith.Length - 1).ToString()));
                        string output = "";
                        try
                        {
                            int intPosFrom = Convert.ToInt32(posFrom);
                            int intPosTo = Convert.ToInt32(posTo);
                            int intLength = intPosTo - intPosFrom + 1;
                            if (intLength + intPosFrom > replaceWith.Length)
                                intLength = replaceWith.Length - intPosFrom;
                            if (intLength > 0)
                                output = replaceWith.Substring(intPosFrom, intLength);
                        }
                        catch
                        {
                            output = replaceWith;
                        }
                        finally
                        {
                            textLine = textLine.Replace(patternFound, output);
                            patternHandled.Add(patternFound);
                        }
                    }
                }
                return textLine;
            }
            else
            {
                return textLine;
            }
        }

        public static string calcString(string formula)
        {
            Regex regex = new Regex("([0-9]+)([+-])([0-9]+)"); //(.*)([+-])(.*)
            if (regex.IsMatch(formula))
            {
                try
                {
                    int A = Convert.ToInt32(regex.Match(formula).Groups[1].ToString());
                    int B = Convert.ToInt32(regex.Match(formula).Groups[3].ToString());
                    string calcType = regex.Match(formula).Groups[2].ToString();
                    if (calcType == "+")
                        return (A + B).ToString();
                    else
                        return (A - B).ToString();
                }
                catch
                {
                    return formula;
                }
            }
            else
            {
                return formula;
            }
        }

        public string[] CreateOutput(string[] DataSet)
        {
            return createOutput(vorlageZeilen.ToArray(), DataSet);
        }

        private string[] createOutput(textZeile[] vorlageZeilen, string[] DataSet)
        {
            List<string> returnOutput = new List<string>();
            string[] tempInput;
            foreach (textZeile thisZeile in vorlageZeilen)
            {
                if (thisZeile is textOnly)
                {
                    textOnly thisTextOnly = thisZeile as textOnly;
                    returnOutput.Add(thisTextOnly.output(variableMarker));
                }
                if (thisZeile is Variable)
                {
                    Variable thisVariable = thisZeile as Variable;
                    int[] requestedData = thisVariable.requestedInputs;
                    tempInput = new string[requestedData.Length];
                    for(int i = 0;i<requestedData.Length;i++)
                    {
                        if(requestedData[i] >= 0 && requestedData[i] < DataSet.Length)
                            tempInput[i] = DataSet[requestedData[i]];
                    }
                    if(requestedData.Length == tempInput.Length)
                        returnOutput.Add(thisVariable.output(tempInput,variableMarker));
                }
                if (thisZeile is ifBlock)
                {
                    ifBlock thisIfBlock = thisZeile as ifBlock;
                    int[] requestedData = thisIfBlock.requestedInputs;
                    tempInput = new string[requestedData.Length];
                    for (int i = 0; i < requestedData.Length; i++)
                    {
                        if (requestedData[i] >= 0 && requestedData[i] < DataSet.Length)
                            tempInput[i] = DataSet[requestedData[i]];
                    }
                    if (requestedData.Length == tempInput.Length)
                    {
                        string[]tempOutput = createOutput(thisIfBlock.output(tempInput, variableMarker),DataSet);
                        foreach (string thisLine in tempOutput)
                            returnOutput.Add(thisLine);
                    }
                }
            }
            return returnOutput.ToArray();
        }
    }

    interface textZeile
    {
        int[] requestedInputs{get;}
    }

    class textOnly : textZeile
    {
        private string textZeile;
        public textOnly(string textZeile)
        {
            this.textZeile = textZeile;
        }
        public string output(string variableMarker)
        {
            return textZeile;
        }
        public int[] requestedInputs
        {
            get { return new int[]{-1}; }
        }
    }

    class Variable : textZeile
    {
        private string textZeile;
        private Dictionary<int, string> inputColumns;
        public Variable(string textZeile, Dictionary<int, string> columns)
        {
            this.textZeile = textZeile;
            this.inputColumns = columns;
        }
        public string output(string[] input, string variableMarker)
        {
            string textInput = textZeile;
            int counter = 0;
            foreach (KeyValuePair<int,string> temp in inputColumns)
            {   
                textInput = vorlage.patternSearch(textInput,variableMarker,temp.Value,input[counter++]);
            }
            return textInput;
        }
        public int[] requestedInputs
        {
            get
            {
                int[] tempReturn = new int[inputColumns.Count()];
                int counter = 0;
                foreach(KeyValuePair<int,string> key in inputColumns)
                    tempReturn[counter++] = key.Key;
                return tempReturn;
            }
        }
    }

    class ifBlock : textZeile
    {
        private vorlage vorlage;
        private Dictionary<int, string> inputColumns;
        private List<textZeile> matchCase;
        private List<textZeile> elseCase;
        private string ifCase;
        public ifBlock(vorlage vorlage, string[] textZeilen, Dictionary<int, string> columns)
        {
            this.vorlage = vorlage;
            this.inputColumns = columns;
            int ifBlocks = 0;
            bool elseBlock = false;
            List<string> matchCaseZeilen = new List<string>();
            List<string> elseCaseZeilen = new List<string>();
            foreach (string thisZeile in textZeilen)
            {
                if (thisZeile.StartsWith("/IF "))
                {
                    if (ifBlocks == 0)
                    {
                        ifCase = thisZeile.Substring(3);
                        elseBlock = false;
                    }
                    ifBlocks++;
                }
                else if (thisZeile.StartsWith("/ELSE") && ifBlocks == 1)
                {
                    elseBlock = true;
                }
                else if (thisZeile.StartsWith("/FI"))
                {
                    ifBlocks--;
                }
                else
                {
                    if (ifBlocks > 0)
                    {
                        if (elseBlock)
                            elseCaseZeilen.Add(thisZeile);
                        else
                            matchCaseZeilen.Add(thisZeile);
                    }
                }
            }
            matchCase = vorlage.analyse(matchCaseZeilen.ToArray());
            elseCase = vorlage.analyse(elseCaseZeilen.ToArray());
        }

        public textZeile[] output(string[] input, string variableMarker)
        {
            string ifCaseInput = ifCase;
            int counter = 0;
            foreach (KeyValuePair<int, string> temp in inputColumns)
            {
                ifCaseInput = vorlage.patternSearch(ifCaseInput, variableMarker, temp.Value, input[counter++]);
            } 
            if(exploreIfCase(ifCaseInput))
            {
                return matchCase.ToArray();
            }
            else
            {
                return elseCase.ToArray();
            }
        }

        private bool exploreIfCase(string ifCase)
        {
            try
            {
                ifCase = ifCase.Trim();
                string compareType;
                string[] comparePart;
                if (ifCase.Contains(@"=="))
                    compareType = @"==";
                else if (ifCase.Contains(@"!="))
                    compareType = @"!=";
                else if (ifCase.Contains(@"<="))
                    compareType = @"<=";
                else if (ifCase.Contains(@">="))
                    compareType = @">=";
                else if (ifCase.Contains(@"<>"))
                    compareType = @"<>";
                else if (ifCase.Contains(@">"))
                    compareType = @">";
                else if (ifCase.Contains(@"<"))
                    compareType = @"<";
                else
                    return false;
                string[] compareArray = new string[1];

                comparePart = ifCase.Split(new string[] { compareType }, 2, StringSplitOptions.RemoveEmptyEntries);
                List<double> douComparePart = new List<double>();
                Regex regexDouble = new Regex(@"^[\-+]?[0-9]+$", RegexOptions.IgnoreCase);
                for (int i = 0; i < 2; i++)
                {
                    comparePart[i] = comparePart[i].Trim();
                    comparePart[i] = comparePart[i].Trim("\"".ToCharArray());
                    if (regexDouble.IsMatch(comparePart[i]))
                        douComparePart.Add(Convert.ToDouble(comparePart[i]));
                }
                switch (compareType)
                {
                    case @"==":
                        if (comparePart[0] == comparePart[1])
                            return true;
                        else
                            return false;
                    case @"!=":
                        if (comparePart[0] != comparePart[1])
                            return true;
                        else
                            return false;
                    case @"<=":
                        if (douComparePart.Count != 2)
                            return false;
                        if (douComparePart[0] <= douComparePart[1])
                            return true;
                        else
                            return false;
                    case @">=":
                        if (douComparePart.Count != 2)
                            return false;
                        if (douComparePart[0] >= douComparePart[1])
                            return true;
                        else
                            return false;
                    case @"<>":
                        if (douComparePart.Count != 2)
                            return false;
                        if (douComparePart[0] < douComparePart[1] || douComparePart[0] > douComparePart[1])
                            return true;
                        else
                            return false;
                    case @">":
                        if (douComparePart.Count != 2)
                            return false;
                        if (douComparePart[0] > douComparePart[1])
                            return true;
                        else
                            return false;
                    case @"<":
                        if (douComparePart.Count != 2)
                            return false;
                        if (douComparePart[0] < douComparePart[1])
                            return true;
                        else
                            return false;
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public int[] requestedInputs
        {
            get
            {
                int[] tempReturn = new int[inputColumns.Count()];
                int counter = 0;
                foreach (KeyValuePair<int, string> key in inputColumns)
                    tempReturn[counter++] = key.Key;
                return tempReturn;
            }
        }
    }
}
