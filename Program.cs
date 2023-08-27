// See https://aka.ms/new-console-template for more information
using EventConsolev2;
using System.Collections.Generic;
using System.Text;
List<LineOfEvent> ListOfEvents = new();
string text;
bool InsideEvent = false;
string line;
string EventNum ="";
Char TypeFunction = 'a';
string Function = "";
int[] FunctionParams = { };

using (var streamReader = new StreamReader(@"C:\server\arcanine\map\3.evt", Encoding.UTF8))
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
        }
    }
    if (InsideEvent == true && (line.StartsWith("A") || (line.StartsWith("O") || line.StartsWith("E ")))) // Watch the trailing space of the startswith("E_") its needed.
    {
        LineOfEvent LineEvent = new();
        TypeFunction = line[0];
        Function = line.Split(' ').Skip(1).FirstOrDefault();
        FunctionParams = line.Split(' ').Skip(2).Select(n => Convert.ToInt32(n)).ToArray();
        //This is where we can see the event is finished and should be finished and 
        LineEvent.ID = EventNum;
        LineEvent.TypeFunction = TypeFunction;
        LineEvent.Function = Function;
        LineEvent.FunctionParams = FunctionParams;
        ListOfEvents.Add(LineEvent);
    }


    if (line == "END")
    {
        InsideEvent = false;
    }
}
ReaderOfEvents.Dispose();
 Console.ReadKey();

//LineOfEvent moo = ListOfEvents.Where(x => x.ID == "03");
//Console.Write(moo.Function);