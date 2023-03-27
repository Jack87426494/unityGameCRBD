--只要是一个新的对象（面板）我们那就新建一张表
BasePanel:subClass("MainPanel")

--需要做 实例化面板对象
--为这个面板 处理对应的逻辑 比如按钮点击等等

--初始化该面板 实例化对象 控件事件监听
function MainPanel:Init(name)
    self.base.Init(self, name)
    --为了只添加一次事件监听
    if self.isInitEvent == false then
        print(self:GetControl("btnRole", "Image"))
        self:GetControl("btnRole", "Button").onClick:AddListener(function()
            self:BtnRoleClick()
        end)

        self.isInitEvent = true
    end
    
end

function MainPanel:BtnRoleClick()
    --print(123123)
    --print(self.panelObj)
    --等我们写了背包面板 
    --在这显示我们的 背包面板
    BagPanel:ShowMe("BagPanel")
end