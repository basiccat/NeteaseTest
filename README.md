# TestProject
Test project for netease mini game.
//
目前使用GameManager下的Pause等函数来实现在游戏中的暂停和继续，同时设定GameManager的判定变量和Time.timescale
而角色死亡的时只是改变判定变量，将涉及到time的所有函数加上if条件判定，此时怪物的动画正常播放，日后可修改为庆祝动画。