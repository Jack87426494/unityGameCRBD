--用到之前讲过的知识 Object
--生成一个table 集成Object 主要目的是要它里面实现的 继承方法subClass和new
Object:subClass("ItemGrid")
--“成员变量”
ItemGrid.obj = nil
ItemGrid.imgIcon = nil
ItemGrid.Text = nil
--成员函数
--实例化格子对象
function ItemGrid:Init(father, posX, posY)
    --实例化格子对象
    self.obj = ABMgr:LoadRes("ui", "ItemGrid");
    --设置父对象
    self.obj.transform:SetParent(father, false)
    --继续设置他的位置
    self.obj.transform.localPosition = Vector3(posX, posY, 0)
    --找控件
    self.imgIcon = self.obj.transform:Find("imgIcon"):GetComponent(typeof(Image))
    self.Text = self.obj .transform:Find("Text"):GetComponent(typeof(Text))
end

--初始化格子信息
--data 是外面传入的 道具信息 里面包含了 id和num
function ItemGrid:InitData(data)
    --通过 道具ID 去读取 道具配置表 得到 图标信息
    local itemData = ItemData[data.id]
    --想要的是data中的 图标信息
    --根据名字 先加载图集 再加载图集中的 图标信息
    local strs = string.split(itemData.icon, "_")
    --加载图集
    local spriteAtlas = ABMgr:LoadRes("ui", strs[1], typeof(SpriteAtlas))
    --加载图标
    self.imgIcon.sprite = spriteAtlas:GetSprite(strs[2])
    --设置它的数量
    self.Text.text = data.num
end

--加自己的逻辑
function ItemGrid:Destroy()
    GameObject.Destroy(self.obj)
    self.obj = nil
end