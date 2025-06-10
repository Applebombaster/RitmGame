namespace GamePlay.Script
{
    public static class Date
    {
        public static float RadiusCircle = 10.0f;
        public static float VolumeSong = 0.1f;
        public static int Difficult = 2;
        public static int[] Records = new int[5];
        public static int PreviousScore;
        public static int MaxScore;
        public static int Combo;
        public static string NameSong = "joakim";
        
    }
    public class SupportClass<T>
    {
        public T[] Item;

        public SupportClass(T[] item)
        {
            Item = item;
        }
    }
    
    public class NoteRecord<T>
    {
        public T[] Note;
        public string[] LongNote;
        public string[] Spinner;

        public NoteRecord(T[] note, string[] longNote, string[] spinner)
        {
            Note = note;
            LongNote = longNote;
            Spinner = spinner;
        }
    }
}