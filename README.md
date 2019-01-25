# CryDialogSystem
基于Unity3D Editor的对话系统

该项目属于在自己前一个项目(Cry Story Editor)的基础上进行修改、移植完成。
主要是考虑到，对话与剧情本身的差别，为了便于两者的区分，同时也为了方便对话本身的编辑与控制，特意新开一个版本，并专门为对话方面的编辑做出了优化。
按照逻辑，不同于CryStoryEditor全局的控制,CryDialogSystem适用于单个对象的处理。因此，结合CryStoryEditor可以更方便地处理NPC之间的对话逻辑控制。

例：
	首先假设游戏中，每个NPC均有自己的默认对话，我们可以在编辑初始时，便将其赋给NPC。（我以前玩过的不少RPG游戏，流程似乎都是这样），当受到剧情的影响，该对话内容需要出现改变，直接再通过StoryEditor更改需要改变对话内容的“主要”NPC，这样，其余普通NPC的对话则不受影响。这样，通过任务故事流程，方便地一步步更新所需对话。

### 详情

在本人个人Blog上，有更详细的说明，地址：[http://cwhisme.coding.me/2016/10/29/%E4%BD%9C%E5%93%81-CryDialogSystem](http://cwhisme.coding.me/2016/10/29/%E4%BD%9C%E5%93%81-CryDialogSystem)

### 界面

![主界面](http://7xp0w0.com1.z0.glb.clouddn.com/%5B2016.10.26%5DMainPage.png)
