// See https://aka.ms/new-console-template for more information
using EventConsolev2;
using System.Text;
List<LineOfEvent> ListOfEvents = new();
string text;
bool InsideEvent = false;
string line;
string EventNum = "";
Char TypeFunction = '-';
string Function = "";
String[] FunctionParams = { };
string ParamAsString = "";
Console.WriteLine("To use this EVT reader, please copy the files of this executable into the folder that contain your EVTs, and only then run this.\rIf you havn't already done this, close this first. ");
Console.WriteLine("Please enter the filename of the EVT to load with its file extension (.evt) - e.g. 1.evt");
String filename = Console.ReadLine();

using (var streamReader = new StreamReader(filename, Encoding.UTF8))
{
    text = streamReader.ReadToEnd();
}

StringReader ReaderOfEvents = new(text);

while ((line = ReaderOfEvents.ReadLine()) != null)
{
    if (InsideEvent == false)
    {
        if (line.StartsWith("EVENT "))
        {

            InsideEvent = true;
            EventNum = line.Split(' ').Skip(1).FirstOrDefault();
            LineOfEvent LineEvent = new();
            LineEvent.ID = EventNum;
            LineEvent.Function = "EVENT " + EventNum;
            ListOfEvents.Add(LineEvent);
        }
    }
    if (InsideEvent == true && (line.StartsWith("A") || (line.StartsWith("O") || line.StartsWith("E ") || line.StartsWith(";")))) // Watch the trailing space of the startswith("E_") its needed.
    {

        LineOfEvent LineEvent = new();
        if (line.StartsWith("A") || (line.StartsWith("O") || line.StartsWith("E ")))
        {
            TypeFunction = line[0];
            Function = line.Split(' ').Skip(1).FirstOrDefault();


            FunctionParams = line.Split(' ').Skip(2).ToArray();
        }
        if (line.StartsWith(";"))
        {
            LineEvent.Note = line;
        }
        if (TypeFunction != '-')
        {
            LineEvent.ID = EventNum;
            LineEvent.TypeFunction = TypeFunction;
            LineEvent.Function = Function;
            LineEvent.FunctionParams = FunctionParams;
        }
        else
        {
            LineEvent.ID = EventNum;
            //LineEvent.TypeFunction = TypeFunction;
            LineEvent.Function = Function;
            LineEvent.FunctionParams = FunctionParams;
        }
        ListOfEvents.Add(LineEvent);
    }


    if (line == "END")
    {

        InsideEvent = false;
        LineOfEvent LineEvent = new();
        LineEvent.ID = EventNum;
        LineEvent.Function = "END";
        ListOfEvents.Add(LineEvent);
    }
}
ReaderOfEvents.Dispose();
Console.WriteLine("Enter event number to find. E.g. 02");
string SearchEvent = Console.ReadLine();

//Console.WriteLine("EVENT " + SearchEvent);*/
while (ListOfEvents.Where(x => x.ID == SearchEvent).Count() == 0)
{
    Console.WriteLine("Nothing found, please try another event number");
    SearchEvent = Console.ReadLine();
}

foreach (LineOfEvent item in ListOfEvents.Where(x => x.ID == SearchEvent))
{

    ParamAsString = "";
    if (item.FunctionParams is not null)
    {
        foreach (string param in item.FunctionParams)
        {
            ParamAsString += param + " ";
        }
        ParamAsString.Trim();
    }

    if (item.Function != "END")
    {
        if (item.Note != "")
        {
            Console.WriteLine(item.Note);

        }
        if (item.Function.StartsWith("EVENT "))
        {
            Console.WriteLine(item.Function);
        }
        else
        {
            if (item.Note != "-")
            {
                Console.WriteLine(item.TypeFunction + " " + item.Function + " " + ParamAsString.ToString());
            }
        }
    }
    else { Console.WriteLine(item.Function); }
}

Console.WriteLine("\r\n\r\nSave output to file - what filename? E.g. test.evt");
string answer = Console.ReadLine();
//
//
//try and save some output
if (answer is not null)
{
    using (StreamWriter writer = new StreamWriter(answer))
    {
        foreach (LineOfEvent item in ListOfEvents)
        {

            if (item.Function.StartsWith("EVENT ") || item.Function.StartsWith("END"))
            {
                writer.WriteLine(item.Function);
                if (item.Function.StartsWith("EVENT "))
                {
                    //  writer.WriteLine(item.Note + "\r\n");
                }
                if (item.Function.StartsWith("END"))
                {
                    writer.WriteLine("\r\n");
                }
            }
            else
            {
                if (item.Note.StartsWith(";"))
                {
                    writer.WriteLine(item.Note);
                }
                else
                {
                    ParamAsString = "";
                    foreach (string param in item.FunctionParams)
                    {
                        ParamAsString += param + " ";
                    }
                    ParamAsString.Trim();
                    writer.WriteLine(item.TypeFunction + " " + item.Function + " " + ParamAsString.ToString());
                }

            }
        }
    }
}