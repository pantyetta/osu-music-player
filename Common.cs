using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer
{
    public class Common
    {
        private static PlayerModel _playerModel= new PlayerModel();
        public PlayerModel PlayerModel { get { return _playerModel; } set { _playerModel = value; } }
    }
}
