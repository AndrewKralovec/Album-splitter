using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; 

namespace TrackSplitter.Models
{
  public class Track {
    public List<Song> items{ get; set; }
    public IFormFile file{ get; set; }
  }
}