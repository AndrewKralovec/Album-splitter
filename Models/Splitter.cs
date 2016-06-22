using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackSplitter.Models
{
  public class Splitter {
    private ProcessStartInfo startInfo = new ProcessStartInfo();
    private string cwd = null ;
    private int status {get;set;}
    public Splitter(string cwd){
            this.cwd = cwd ; 
			startInfo.WorkingDirectory = cwd;
			startInfo.FileName = "avconv";
    }
    public bool split(Track track){
        bool result = false ;
        status = 0 ; 
        foreach (var item in track.items){
            launch(item); 
            status++; 
        }
        if(status == track.items.Count)
            result = true ; 
        return result ; 
    }
    private void launch(Song song){
        startInfo.Arguments = string.Format("-i {0} -ss {1} -t {2} {3}",song.master, song.offset, song.length, song.name); 
        try{
            using (Process exeProcess = Process.Start(startInfo)){
                exeProcess.WaitForExit();
            }
        }
        catch(Exception ex){
            Console.WriteLine(ex);
        }
    }
  }
}