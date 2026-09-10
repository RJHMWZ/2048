# 2048

一个基于 Unity 开发的 2D 版 2048 游戏。项目实现了经典 2048 的核心玩法，并在重构后将规则计算、游戏流程、界面显示、音频、本地存储和场景导航拆分为独立模块，结构更清晰，也更方便后续维护和扩展。

玩家可以使用方向键或 WASD 控制棋盘移动，相同数字会在移动方向上合并并累积分数。当棋盘中首次出现 2048 时触发胜利提示；当棋盘填满且无法继续合并时游戏结束。

## 项目展示
### 主菜单
<img width="296" height="530" alt="封面" src="https://github.com/user-attachments/assets/49029b6d-6a01-417e-a051-9ba39d55e295" />

### 游戏场景
<img width="297" height="534" alt="游戏场景" src="https://github.com/user-attachments/assets/5223f3d0-d948-420f-86dc-c362dbb2fa4d" />


### 试玩视频
[点击观看试玩视频](https://github.com/user-attachments/assets/db1f455c-2152-40e4-b5b8-c8b13c0462c2)
## 项目特点

- 经典 2048 玩法：支持上、下、左、右四方向移动与数字合并
- 清晰的分层结构：`Core`、`Gameplay`、`UI`、`Audio`、`Persistence`、`Infrastructure`
- 统一移动算法：四个方向共用同一套读线、合并和位置映射逻辑
- 输入状态保护：暂停、动画播放、胜利选择和游戏结束时会阻止重复输入
- 本地最高分：使用 `PlayerPrefs` 保存历史最高分
- 音效开关保存：音效状态会持久化到本地
- 完整界面流程：主菜单、游戏界面、暂停、胜利、失败弹窗
- 方块动画：支持移动动画、新方块生成动画和合并缩放动画
- 音频兜底机制：音频资源缺失时可生成轻量级备用音效
- 编辑器调试输出：在 Unity 编辑器中输出当前棋盘状态，便于调试规则逻辑

## 游戏操作

| 操作 | 按键 |
| --- | --- |
| 向上移动 | `↑` 或 `W` |
| 向下移动 | `↓` 或 `S` |
| 向左移动 | `←` 或 `A` |
| 向右移动 | `→` 或 `D` |

## 运行环境

- Unity：`2022.3.62f2`
- 项目类型：Unity 2D
- UI 组件：Unity UI、TextMesh Pro
- 主要语言：C#

建议使用 Unity Hub 安装并打开对应版本的 Unity，避免因版本差异导致资源导入或场景配置异常。

## 快速开始

1. 克隆项目：

   ```bash
   git clone https://github.com/RJHMWZ/2048.git
   ```

2. 使用 Unity Hub 打开项目根目录。

3. 等待 Unity 自动导入依赖和资源。

4. 打开菜单场景：

   ```text
   Assets/Scenes/MenuScene.unity
   ```

5. 点击 Unity 编辑器中的 Play 按钮运行游戏。

## 场景说明

| 场景 | 路径 | 说明 |
| --- | --- | --- |
| 菜单场景 | `Assets/Scenes/MenuScene.unity` | 游戏入口，提供开始游戏和退出游戏 |
| 游戏场景 | `Assets/Scenes/GameScene.unity` | 主游戏场景，包含棋盘、分数、弹窗、输入和音效流程 |

当前构建配置中已包含以上两个场景。

## 项目结构

```text
Assets
├── Prefabs
│   ├── Cell.prefab
│   └── Tile.prefab
├── Resources
│   ├── Audio
│   │   ├── 背景音乐.mp3
│   │   ├── 按钮按下.mp3
│   │   ├── 消除.mp3
│   │   ├── 胜利.mp3
│   │   └── 失败.mp3
│   ├── Fonts
│   └── UI
│       ├── Number.png
│       ├── 图标.png
│       └── 棋盘.png
├── Scenes
│   ├── MenuScene.unity
│   └── GameScene.unity
└── Scripts
    ├── Audio
    │   ├── GameAudioManager.cs
    │   └── ProceduralAudioFactory.cs
    ├── Core
    │   ├── BoardModel.cs
    │   ├── BoardMoveResult.cs
    │   ├── LineMergeUtility.cs
    │   ├── LineMoveResult.cs
    │   ├── LineTileData.cs
    │   ├── MoveDirection.cs
    │   └── TileMoveInfo.cs
    ├── Gameplay
    │   ├── Game2048Input.cs
    │   └── Game2048Manager.cs
    ├── Infrastructure
    │   └── GameManager.cs
    ├── Persistence
    │   ├── GameSaveManager.cs
    │   └── GameSettingsManager.cs
    └── UI
        ├── BoardView.cs
        ├── ButtonSoundFeedback.cs
        ├── GameHUD.cs
        ├── GameResultUI.cs
        ├── MenuManager.cs
        ├── TileMoveAnimator.cs
        └── TileView.cs
```

## 架构说明

### Core

`Core` 层负责 2048 的纯规则逻辑，不直接依赖具体 UI 流程。

- `BoardModel.cs`：维护 4 x 4 棋盘数据，负责随机生成方块、执行移动、判断胜利数字和判断是否还能继续移动。
- `LineMergeUtility.cs`：负责单行或单列的压缩、合并、计分，并生成移动动画所需的位置数据。
- `MoveDirection.cs`：定义移动方向。
- `BoardMoveResult.cs`、`LineMoveResult.cs`、`TileMoveInfo.cs`、`LineTileData.cs`：用于在规则层和表现层之间传递移动结果、分数和动画信息。

重构后，四个方向的移动不再分别维护四套合并逻辑，而是通过 `BoardModel.GetPosition` 统一把方向转换为棋盘坐标，再交给 `LineMergeUtility.Merge` 处理。

### Gameplay

`Gameplay` 层负责游戏流程和玩家输入。

- `Game2048Manager.cs`：协调棋盘模型、UI、音效、分数、最高分、胜利和失败状态。
- `Game2048Input.cs`：监听方向键和 WASD，并在游戏可接收输入时发送移动指令。

`Game2048Manager` 使用 `CanAcceptInput` 集中判断当前是否允许输入，避免在暂停、动画播放、游戏结束或胜利弹窗等待选择时继续移动棋盘。

### UI

`UI` 层负责所有界面显示和动画。

- `BoardView.cs`：将棋盘数据刷新到 16 个固定方块，并负责播放移动动画。
- `TileView.cs`：显示单个数字方块，支持生成动画和合并动画。
- `TileMoveAnimator.cs`：负责移动过程中的临时方块动画。
- `GameHUD.cs`：显示当前分数和最高分。
- `GameResultUI.cs`：管理暂停、胜利、失败面板。
- `MenuManager.cs`：绑定菜单按钮。
- `ButtonSoundFeedback.cs`：为按钮点击提供音效反馈。

### Audio

`Audio` 层负责全局音频。

- `GameAudioManager.cs`：全局音频入口，负责加载背景音乐、按钮音效、移动音效、合并音效、胜利音效和失败音效。
- `ProceduralAudioFactory.cs`：当某些音频资源不存在时，生成简单的备用音效，保证游戏反馈不完全丢失。

### Persistence

`Persistence` 层负责本地数据。

- `GameSaveManager.cs`：保存和读取最高分、音效开关。
- `GameSettingsManager.cs`：为 UI 提供音效设置入口，实际状态由 `GameAudioManager` 统一维护。

### Infrastructure

`Infrastructure` 层负责应用级能力。

- `GameManager.cs`：提供进入游戏、返回菜单和退出游戏的静态方法。

## 核心玩法规则

1. 游戏开始时棋盘随机生成两个数字方块。
2. 玩家选择一个方向后，所有方块会向该方向移动。
3. 同一方向上相邻且数值相同的方块会合并。
4. 每次有效移动后会随机生成一个新的 `2` 或 `4`。
5. 合并产生的数字会累加到当前分数。
6. 当前分数超过历史最高分时会自动保存。
7. 首次合成 `2048` 时触发胜利提示。
8. 当棋盘没有空格，且相邻方块都无法合并时，游戏结束。

## 主要流程

```text
玩家输入
  ↓
Game2048Input 检查是否允许输入
  ↓
Game2048Manager 发起移动流程
  ↓
BoardModel 计算棋盘移动和合并结果
  ↓
BoardView 播放移动动画并刷新方块
  ↓
Game2048Manager 更新分数、最高分和游戏状态
  ↓
GameResultUI 根据状态显示胜利或失败界面
```

## 构建项目

1. 在 Unity 中打开项目。
2. 进入 `File > Build Settings`。
3. 确认场景列表中包含：

   ```text
   Assets/Scenes/MenuScene.unity
   Assets/Scenes/GameScene.unity
   ```

4. 选择目标平台，例如 Windows、macOS、WebGL 或 Android。
5. 点击 `Build` 生成可运行版本。

## 适合学习的内容

- Unity UI 与 TextMesh Pro 的基础使用
- 2048 棋盘数据结构设计
- 四方向移动和合并算法的统一抽象
- 游戏状态管理与输入拦截
- `PlayerPrefs` 本地数据保存
- 简单音频管理和按钮音效绑定
- 协程动画在 UI 方块移动中的使用
- 游戏逻辑和显示逻辑分层

## 后续优化方向

- 增加移动端滑动操作
- 增加撤销上一步功能
- 增加更多主题皮肤
- 增加排行榜或通关统计
- 增加核心规则单元测试
- 增加 WebGL 在线试玩页面
- 优化不同分辨率下的 UI 适配

## 许可证

当前仓库暂未声明开源许可证。如果希望其他人明确了解项目是否允许使用、修改或分发，建议后续补充 `LICENSE` 文件。
