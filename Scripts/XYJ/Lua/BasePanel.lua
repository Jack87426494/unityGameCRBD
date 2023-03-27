--利用面向对象
Object:subClass("BasePanel")

BasePanel.panelObj = nil
--相当于模拟一个字典 键为 控件名 值为控件本身
BasePanel.controls = {}
--事件监听标识
BasePanel.isInitEvent = false

function BasePanel:Init(name)
    if self.panelObj == nil then
        --公共的实例化对象的方法
        self.panelObj = ABMgr:LoadRes("ui", name, typeof(GameObject))
        self.panelObj.transform:SetParent(Canvas, false)
        --GetComponentsInChildren
        --找到所有UI控件 存起来
        print(self.panelObj)
        local allControls = self.panelObj:GetComponentsInChildren(typeof(UIBehaviour))
        --如果存入一些对于我们来说没用UI控件 
        --为了避免 找各种无用控件 我们定一个规则 拼面板时 控件命名一定按规范来
        --Button btn名字
        --Toggle tog名字
        --Image img名字
        --ScrollRect sv名字
        for i = 0, allControls.Length-1 do
            local controlName = allControls[i].name
            --按照名字的规则 去找控件 必须满足命名规则 才存起来
            if string.find(controlName, "btn") ~= nil or 
               string.find(controlName, "tog") ~= nil or 
               string.find(controlName, "img") ~= nil or 
               string.find(controlName, "sv") ~= nil or
               string.find(controlName, "txt") ~= nil then
                --为了让我们在得的时候 能够 确定得的控件类型 所以我们需要存储类型
                --利用反射 Type 得到 控件的类名 
                local typeName = allControls[i]:GetType().Name
                --避免出现一个对象上 挂在多个UI控件 出现覆盖的问题 
                --都会被存到一个容器中 相当于像列表数组的形式
                --最终存储形式 
                --{ btnRole = { Image = 控件, Button = 控件 },
                --  togItem = { Toggle = 控件} }
                if self.controls[controlName] ~= nil then
                    --通过自定义索引的形式 去加一个新的 “成员变量”
                    self.controls[controlName][typeName] = allControls[i]
                else
                    self.controls[controlName] = {[typeName] = allControls[i]}
                end
            end
        end
    end
end

--得到控件 根据 控件依附对象的名字 和 控件的类型字符串名字 Button Image Toggle
function BasePanel:GetControl(name, typeName)
    if self.controls[name] ~= nil then
        local sameNameControls = self.controls[name]
        if sameNameControls[typeName] ~= nil then
            return sameNameControls[typeName]
        end
    end
    return nil
end

function BasePanel:ShowMe(name)
    self:Init(name)
    self.panelObj:SetActive(true)
end

function BasePanel:HideMe()
    self.panelObj:SetActive(false)
end
