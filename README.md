# TestProject
Test project for netease mini game.
//
修复一些bug，完成了游戏的完整流程以及初步的数值设计，完成了对话剧情，完成了定身技能的实现。UI待整合。
//
整合了初步的UI，增加了boss的技能，添加了音效，待完成；
//
目前使用GameManager下的Pause等函数来实现在游戏中的暂停和继续，同时设定GameManager的判定变量和Time.timescale
而角色死亡的时只是改变判定变量，将涉及到time的所有函数加上if条件判定，此时怪物的动画正常播放，日后可修改为庆祝动画。
