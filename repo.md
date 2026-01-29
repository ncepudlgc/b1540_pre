# b1540_pre

## Repo简介

Space Kitsune是一个Unity开发的Starfox风格的3D太空射击游戏原型。玩家控制一艘飞船在轨道上前进，通过移动瞄准器（reticle）来瞄准和射击敌人。

**主要功能：**
- 玩家飞船自动前进，通过输入控制瞄准器位置
- 飞船会平滑地朝向瞄准器方向旋转
- 射击系统：玩家可以发射弹丸攻击敌人
- 敌人生成系统：随机生成不同类型的敌人
- 相机跟随系统：相机从后方跟随玩家
- 能量系统：支持加速和减速功能，消耗能量资源

**技术栈：**
- Unity 6000.0.47f1
- C#脚本
- Unity Input System（支持键盘和手柄）
- Unity Physics系统（Rigidbody）

**项目结构：**
- `Assets/PlayerController.cs`: 玩家控制器，处理移动、射击、瞄准器控制
- `Assets/Projectile.cs`: 弹丸脚本，处理弹丸物理和碰撞
- `Assets/CameraController.cs`: 相机控制器，跟随玩家
- `Assets/GameManager.cs`: 游戏管理器，处理敌人生成
- `Assets/Enemy.cs`: 敌人基础脚本
- `Assets/SniperEnemy.cs`, `Assets/ZippyEnemy.cs`: 特殊敌人类型

## 题目Prompt

a few issues. Player projectiles should be much faster so that I can shoot enemies more easily. Theres something wrong with the reticle, I want to increase the distance but whenever I increase the distance the ship starts jittering like crazy and I can't play. The reticle rotation should remain consistent with the ships rotation I should not be seeing the reticle from an angle when I turn extremely. Also the reticle drifts back to center when I am closer to the edges of the movement boundaries, that shouldn't happen. And finally the ship jitters forward/back when brake/boost is empty, ship should return to base speed with smooth movement when I run out of boost.

## PR链接

待创建
