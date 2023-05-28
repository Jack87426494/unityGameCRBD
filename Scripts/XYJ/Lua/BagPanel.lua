--一个面板对应一个表
BasePanel:subClass("BagPanel")

BagPanel.Content = nil
--用来存储当前 显示的格子
BagPanel.items = {}
--用来存储当前显示的页签 类型 避免重复刷新
BagPanel.nowType = -1

--"成员方法"
--初始化方法
function BagPanel:Init(name)
    self.base.Init(self, name)

    if self.isInitEvent == false then
        --找到没有挂在UI控件的 对象 还是需要手动去找
        self.Content = self:GetControl("svBag", "ScrollRect").transform:Find("Viewport"):Find("Content")
        --加事件
        --关闭按钮
        self:GetControl("btnClose", "Button").onClick:AddListener(function()
            self:HideMe()
        end)
        --单选框事件
        --切页签
        --toggle 对应委托 是 UnityAction<bool>
        self:GetControl("togEquip", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(1)
            end
        end)
        self:GetControl("togItem", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(2)
            end
        end)
        self:GetControl("togGem", "Toggle").onValueChanged:AddListener(function(value)
            if value == true then
                self:ChangeType(3)
            end
        end)

        self.isInitEvent = true
    end
end
--显示隐藏
function BagPanel:ShowMe(name)
    self.base.ShowMe(self, name)
    --第一次打开是 更新数据
    if self.nowType == -1 then
        self:ChangeType(1)
    end
end

--逻辑处理函数 用来切页签的
--type 1装备 2道具 3宝石
function BagPanel:ChangeType(type)
    --判断如果已经是该页签 就别更新了
    if self.nowType == type then
        return
    end
    --切页  根据玩家信息 来进行格子创建

    --更新之前 把老的格子删掉 BagPanel.items
    for i = 1, #self.items do
        --销毁格子对象
        self.items[i]:Destroy()
    end
    self.items = {}

    --再根据当前选择的类型 来创建新的格子 BagPanel.items
    --要根据 传入的 type 来选择 显示的数据
    local nowItems = nil
    if type == 1 then
        nowItems = PlayerData.equips
    elseif type == 2 then
        nowItems = PlayerData.items
    else
        nowItems = PlayerData.gems
    end

    --创建格子
    for i = 1, #nowItems do
        --根据数据 创建一个格子对象
        local grid = ItemGrid:new()
        --要实例化对象 设置位置
        grid:Init(self.Content, (i-1)%4*175, math.floor((i-1)/4)*175)
        --初始化它的信息 数量和图标
        grid:InitData(nowItems[i])    
        --把它存起来
        table.insert(self.items, grid)
    end
end