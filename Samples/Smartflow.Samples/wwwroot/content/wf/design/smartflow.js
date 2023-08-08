 (function (initialize) {

    var config = {
        root: 'workflow',
        from: 'from',
        transition: 'transition',
        id: 'id',
        name: 'name',
        to: 'destination',
        expression: 'expression',
        horizontal: 'horizontal',
        vertical: 'vertical',
        anchor: 'anchor',
        layout: 'layout',
        category: 'category',
        order:'order',
        x: 'x',
        y: 'y',
        length: 'length',
        url: 'url',
        rule: 'rule',
        x3: 'x3',
        y3: 'y3',
        textRect:'textRect'
    };

    Array.prototype.remove = function (dx, to) {
        this.splice(dx, (to || 1));
     }

    Array.prototype.insert = function (pos, item) {
         this.splice(pos, 0, item);
     }

    Array.prototype.contains = function (va) {
         for (var i = 0; i < this.length; i++) {
             if (this[i] == va) {
                 return true;
             }
         }
         return false;
     }

    Function.prototype.extend = function (Parent, Override) {
        function F() { }
        F.prototype = Parent.prototype;
        this.prototype = new F();
        this.prototype.constructor = this;
        this.base = {};
        this.base.Parent = Parent;
        this.base.Constructor = Parent;
        if (Override) {
            $.extend(this.prototype, Override);
        }
    }

    function Draw(option) {
        this.draw = SVG(option.container);
        this.support = (!!window.ActiveXObject || "ActiveXObject" in window);
        this.drawOption = $.extend({
            backgroundColor: '#000',
            strokeColor: '#000'
        }, option);
        this.source = undefined;
        this._shared = undefined;
        this._decision = undefined;
        this._init();
    }

    Draw.id = 31;
    Draw._proto_NC = {};
    Draw._proto_LC = {};
    Draw._proto_LC_QC = [];
    Draw._proto_LC_TC = {};
    Draw._proto_RC = [];
    Draw._proto_Cc = {};

    Draw.remove = function (elements) {
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            if (element) {
                var instance = Draw._proto_LC[element.id];
                Draw.removeById(element.id);
                instance.remove();
            }
        }
    }
    Draw.removeById = function (id) {
        for (var i = 0, len = Draw._proto_RC.length; i < len; i++) {
            if (Draw._proto_RC[i].id == id) {
                Draw._proto_RC.remove(i);
                break;
            }
        }
    }
    Draw.findById = function (elementId, propertyName) {
        var elements = [];
        $.each(Draw._proto_RC, function () {
            var self = this;
            if (propertyName) {
                if (this[propertyName] === elementId) {
                    elements.push(this);
                }
            } else {
                $.each(["to", "from"], function () {
                    if (self[this] === elementId) {
                        elements.push(self);
                    }
                });
            }
        });
        return elements;
     }
    Draw.genId = function (id) {
        return Shape.isNotEmpty(id) && 0 != id && '0' != id ? id : this.id++;
    }

    Draw.getEvent = function (evt) {
        var n = evt.target;
        if (n.nodeName === 'svg') {
            return null;
        }
        else {
            return (evt.target.correspondingUseElement || evt.target);
        }
    }

    Draw.angle = function (a, b) {
         var dffx = b.x - a.x;
         var diffy = b.y - a.y;
         var angle = Math.atan2(diffy, dffx) * (180 / Math.PI);
         //console.log(angle);
         return Math.abs(Math.floor(angle));
     }

    Draw.getPosition = function (layout) {
        var pos = layout.split(' ');
        return {
            x: Number(pos[0]),
            y: Number(pos[2]),
            disX: Number(pos[1]),
            disY: Number(pos[3])
        };
    }
    Draw.prototype._init = function () {
        var self = this,
            dw = self.draw;

        self.draw.mouseup(function (e) {
            if (Draw._proto_LC_QC.length === 0) {
                dw.off('mousemove');
            }
        });
        self.draw.dblclick(function (evt) {
            if (Draw._proto_LC_QC.length > 0) {
                var node = Draw.getEvent(evt),
                    check = (node != null);
                if (check) {
                    var nodeName = node.nodeName,
                        nodeId = node.id;
                    if ((nodeName == 'rect' || nodeName == 'use') && self.source) {
                        var nt = Draw._proto_NC[nodeId],
                            nf = Draw._proto_NC[self.source.id];
                        var x = Draw.getClientX(evt),
                            y = Draw.getClientY(evt);
                        if (!nt.check(nf) && nt.bound(x, y)) {
                            var c = self._end.call(self, node, evt);
                            var l = Draw._proto_LC_QC.shift();
                            if (!c) {
                                delete self._shared;
                                delete self.source;
                                l.addArrowTriangle();
                                l.addAnchor();
                            } else {
                                l.remove();
                            }
                        }
                    }
                } else {
                    var instance = Draw._proto_LC_QC[0];
                    instance.coords.push([Draw.getClientX(evt), Draw.getClientY(evt)]);
                    instance.plot(instance.coords);
                }
            }
        });

        self._initEvent();
        self._decision = dw.group()
            .add(dw.path("M0 0 50 -25 100 0 50 25z").fill("#f06"));
        dw.defs().add(self._decision);
    }

    Draw.prototype._initEvent = function () {
        var self = this;
        self.draw.each(function () {
            switch (this.type) {
                case "rect":
                case "use":
                case "circle":
                    this.off('mousedown');
                    break;
                default:
                    break;
            }
        });
     }

    Draw.getClientX = function (evt) {
        return evt.offsetX;
    }

     Draw.getClientY = function (evt) {
        return evt.offsetY;
     }

    Draw.prototype._drag = function (evt, o) {
        var self = this,
            nx = Draw._proto_NC[self.id()];
        evt.preventDefault();
        nx.disX = Draw.getClientX(evt) - self.x();
        nx.disY = Draw.getClientY(evt) - self.y();
        o.draw.on('mousemove', function (d) {
            d.preventDefault();
            nx.move(self, d);
        });
     }
    Draw.prototype._start = function (node, evt) {

        var nodeName = node.nodeName,
            nodeId = node.id;

        if (nodeName == 'rect' || nodeName == 'use') {
            var instance = Draw._proto_NC[nodeId];
            var result = instance.bound(Draw.getClientX(evt), Draw.getClientY(evt));
            if (result) {

                this._shared = new Line();
                this._shared.drawInstance = this;
                var x = result.x;
                var y = result.y;
                this._shared.x1 = x;
                this._shared.y1 = y;
                this._shared.x2 = x;
                this._shared.y2 = y;
                this._shared.draw();
                this.source = {
                    id: nodeId
                };

                return true;
            }
        }

        return false;
    }
    Draw.prototype._join = function (evt) {
         if (this._shared) {
             var last = this._shared.last2();
             var x = Draw.getClientX(evt);
             var y = Draw.getClientY(evt);
             var dffx = x - last.x;
             var diffy = y - last.y;
             var a = Math.atan2(diffy, dffx) * (180 / Math.PI);
             var aw = 360 - a + 90;
             var tx = aw > 0 ? 360 + aw : aw;
             var ta = tx % 360;
             var abs = ta / 360 * 24;
             var dir = Math.floor(abs);
             var offset = 5;
             var lrt = [17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6];
             var lt = [17, 16, 15, 14, 13, 12];
             var bl = [23, 22, 21, 20, 19, 18];
             if (lrt.contains(dir)) {
                 this._shared.y2 = y + offset;
                 this._shared.x2 = lt.contains(dir) ? x + offset : x - offset;
             } else {
                 this._shared.x2 = bl.contains(dir) ? x + offset : x - offset;
                 this._shared.y2 = y - offset;
             }
             this._shared.move();
         }
     }

    Draw.prototype._end = function (node, evt) {
         var self = this,
             nodeId = node.id;
         var to = SVG.get(nodeId),
             from = SVG.get(self.source.id),
             instance = self._shared;

         self.source.to = nodeId;
         instance.move(evt);

         var totalPoints = instance.getPoints();
         var last = instance.last();
         var first = instance.first();
         Draw._proto_RC.push({
             id: instance.$id,
             from: self.source.id,
             to: nodeId,
             ox2: last.x - to.x(),
             oy2: last.y - to.y(),
             ox1: first.x - from.x(),
             oy1: first.y - from.y()
         });

         return totalPoints.length === 2 && last.x == first.x && last.y === first.y;
     }

    Draw.prototype.join = function () {
        var self = this;
        this._initEvent();
        this.draw.off('mousedown').on('mousedown', function (evt) {
            var node = Draw.getEvent(evt);
            if (node != null) {
                if (self._start.call(self, node, evt)) {
                    self.draw.on('mousemove', function (e) {
                        self._join.call(self, e);
                    });
                    self.draw.off('mousedown');
                }
            }
        });
     }

    Draw.prototype.select = function () {
        this._initEvent();
        this.draw.off('mousedown');
        var self = this;
        self.draw.each(function () {
            var el = this;
            switch (el.type) {
                case "rect":
                case "use":
                case "circle":
                    el.mousedown(function (evt) {
                        self._drag.call(this, evt, self);
                    });
                    break;
                default:
                    break;
            }
        });
     }

    Draw.prototype.create = function (category, after) {
        var instance;
        switch (category) {
            case "start":
                instance = new Start();
                break;
            case "end":
                instance = new End();
                break;
            case "decision":
                instance = new Decision();
                break;
            case "fork":
                instance = new Fork();
                break;
            case "join":
                instance = new Join();
                break;
            default:
                instance = new Node();
                break;
        }
        instance.drawInstance = this;
        instance.x = Math.floor(Math.random() * 200 + 1);
        instance.y = Math.floor(Math.random() * 200 + 1);
        if (!after) {
            instance.draw();
        }
        instance.id = Draw.genId(instance.id);
        return instance;
    }

    Draw.prototype.createXmlDocument = function () {
        var docXml;
        if (this.support) {
            docXml = new ActiveXObject("Microsoft.XMLDOM");
        }
        else if (document.implementation && document.implementation.createDocument) {
            docXml = document.implementation.createDocument('', '', null);
        }
        if (docXml) {
            docXml.async = false;
        }
        return docXml;
    }

    Draw.prototype.serialize = function (doc) {
        return this.support ? doc.xml : new XMLSerializer().serializeToString(doc);
     }

    Draw.prototype.export = function () {
        if (!this.validate()) return;
        var doc = this.createXmlDocument();
        var root = doc.createElement(config.root);
        doc.appendChild(root);
        $.each(Draw._proto_NC, function () {
            if (![config.anchor, config.textRect, config.horizontal,config.vertical].contains(this.category)) {
                this.export(doc, root);
            }
        });
        return encodeURI(this.serialize(doc));
    }

    Draw.prototype.validate = function () {
        var validateCollection = [];

        $.each(Draw._proto_NC, function () {
            var self = this;
            if (self.validate&&!self.validate() ) {
                validateCollection.push(false);
            }
        });

        return !(validateCollection.length > 0 || Draw._proto_RC.length === 0);
     }

    Draw.prototype.import = function (structure) {
        var dwInstance = this,
            root = new XML(structure, dwInstance.support).root;
        var data = root.workflow.nodes;
        function findUID(destination) {
            var id;
            for (var i = 0, len = data.length; i < len; i++) {
                var node = data[i];
                if (destination == node.id) {
                    id = node.$id;
                    break;
                }
            }
            return id;
        }

        $.each(data, function () {
            var node = this;
            node.category = node.category.toLowerCase();
            var instance = dwInstance.create(node.category, true);
            $.extend(instance,Draw.getPosition(node.layout));
            instance.draw();
            $.extend(instance, node);
            instance.updateContent && instance.updateContent(node.name);
            instance.id = Draw.genId(node.id);
            node.$id = instance.$id;
        });

        $.each(data, function () {
            var self = this;
            self.transition = (self.transition || []);
            $.each(self.transition, function () {
                var transition = new Line();
                transition.drawInstance = dwInstance;
                $.extend(transition, this);
                //temp 
                transition.x3 = parseFloat(transition.x3);
                transition.y3 = parseFloat(transition.y3);
                transition.draw(this.layout);
                var destinationId = findUID(transition.destination),
                    destination = SVG.get(destinationId),
                    from = SVG.get(self.$id);
                var last = transition.last(),
                    first = transition.first();
                transition.addText();
                transition.addAnchor();
                transition.addArrowTriangle();
                transition.id = Draw.genId(this.id);
                transition.updateContent(this.name);
                Draw._proto_RC.push({
                    id: transition.$id,
                    from: self.$id,
                    to: destinationId,
                    ox2: last.x - destination.x(),
                    oy2: last.y - destination.y(),
                    ox1: first.x - from.x(),
                    oy1: first.y - from.y()
                });
            });
        });

        Draw._proto_LC_QC = [];
    }

    function Element(id,name, category,dw) {
        this.$id = id;
        this.id = 0;
        this.name = name;
        this.category = category;
        this.drawInstance = dw;
    }

    Element.prototype = {
        constructor: Element,
        draw: function () {
            var el = SVG.get(this.$id);
            this.bindEvent.call(el, this);
        },
        bindEvent: function (el) {
            this.dblclick(function (evt) {
                evt.preventDefault();
                var node = Draw._proto_NC[this.id()];
                node.edit.call(this, evt, el);
                return false;
            });
            this.mouseover(function (evt) {
                evt.preventDefault();
                return false;
            });
            this.mouseout(function (evt) {
                evt.preventDefault();
                return false;
            });
        }
    }

    function Shape(id, name, category, dw) {
        Shape.base.Constructor.call(this, id, name, category, dw);
        this.group = [];
        this.organization = [];
        this.action = [];
        this.actor = [];
        this.carbon = [];
        this.rule = '';
        this.tickness = 20;
    }

    Shape.extend(Element, {
        constructor: Shape,
        check: function (nf) {
            return (nf.category === 'end' || this.category === 'start')
                || (nf.category === 'start' && this.category === 'end');
        },
        edit: function (evt, el) {
            if (evt.ctrlKey && evt.altKey) {
                var id = this.id(),
                    node = Draw._proto_NC[id];

                Draw.remove(Draw.findById(id));
                if (node.brush) {
                    node.brush.remove();
                }
                SVG.get(id).remove();
                delete Draw._proto_NC[id];
            } else {
                var nx = Draw._proto_NC[this.id()];
                el.drawInstance.drawOption['dblClick']
                    && el.drawInstance.
                        drawOption['dblClick'].call(this, nx);
            }
        },
        move: function () {
            Line.move.call(this, this);
        },
        getTransitions: function () {
            var elements = Draw.findById(this.$id, config.from),
                lineCollection = [];
            $.each(elements, function () {
                lineCollection.push(Draw._proto_LC[this.id]);
            });
            return lineCollection;
        }
    });

    Shape.isNotEmpty = function (value) {
         return !(value === null || value === undefined || value === '' || value ==='undefined');
    }

    Shape.prototype.export = function (doc, root) {
        var self = this;
        var node = doc.createElement(self.category);
        node.setAttribute(config.id, self[config.id]);
        node.setAttribute(config.name, self[config.name]);
        node.setAttribute(config.layout, self.x + ' ' + self.disX + ' ' + self.y + ' ' + self.disY);
        node.setAttribute(config.category, self.category);
        if (Shape.isNotEmpty(self.rule)) {
            node.setAttribute(config.rule, self.rule);
        }
        var attrObject = {
            group: self.group,
            organization: self.organization,
            action: self.action,
            actor: self.actor
        };

        for (var propertyName in attrObject) {
            var attrs = attrObject[propertyName];
            $.each(attrs, function () {
                var attr = doc.createElement(propertyName);
                attr.setAttribute(config.id, this.id);
                attr.setAttribute(config.name, this.name);
                node.appendChild(attr);
            });
        }
      
        var elements = Draw.findById(self.$id, config.from);
        $.each(elements, function () {
            if (this.from === self.$id) {
                const
                    L = Draw._proto_LC[this.id],
                    N = Draw._proto_NC[this.to],
                    LT = Draw._proto_LC_TC[this.id];

                const transition = doc.createElement(config.transition);
                transition.setAttribute(config.name, LT.name);
                transition.setAttribute(config.to, N.id);
                transition.setAttribute(config.layout, L.getPoints().join(" "));
                transition.setAttribute(config.x3, LT.x);
                transition.setAttribute(config.y3, LT.y);
                transition.setAttribute(config.id, L.id);
                transition.setAttribute(config.order, L.order);

                if (Shape.isNotEmpty(L.url)) {
                    transition.setAttribute(config.url, L.url);
                }
                if (self.category === 'decision') {
                    const expression = doc.createElement(config.expression);
                    expression.appendChild(doc.createCDATASection(L.expression));
                    transition.appendChild(expression);
                }
                node.appendChild(transition);
            }
        });

        root.appendChild(node);
    }

    function Anchor(x, y, line) {
        this.x = x;
        this.y = y;
        this.r = 10;
        this.disX = 0;
        this.disY = 0;
        this.border = 2;
        this.line = line;
    }

    Anchor.extend(Shape, {
        draw: function () {
            const self = this,
                parentGroup = SVG.get(self.line.$id).parent();
            const circle = parentGroup.circle(self.r).fill("#33f1ea").attr({ opacity: 0, cursor: 'move' });
            Anchor.base.Constructor.call(this, circle.id(), "标记位", config.anchor, self.line.drawInstance);
            circle.attr({ cx: self.x, cy: self.y });
            Draw._proto_NC[self.$id] = self;
            return Shape.base.Parent.prototype.draw.call(this);
        },
        move: function (element, evt) {
            var self = this;
            self.x = Draw.getClientX(evt);
            self.y = Draw.getClientY(evt);
            element.attr({ cx: self.x, cy: self.y });
            self.line.redraw();
        },
        adjust: function (x, y) {
            const self = this;
            const el = SVG.get(self.$id);
            el.attr({ cx: x, cy: y });
        },
        remove: function (i) {
            this.line.anchors.remove(i);
            SVG.get(this.$id).remove();
        },
        removeElement: function () {
            SVG.get(this.$id).remove();
        },
        bindEvent: function (el) {
            this.mousedown(function (evt) {
                el.drawInstance._drag.call(this, evt, el.drawInstance);
            });
            this.mouseup(function () {
                const self = Draw._proto_NC[this.id()], L = self.line;
                let adjustArray = L.autoSegment();
                let points = L.getPoints();
                for (var s = 0; s < adjustArray.length; s++) {
                    let st = adjustArray[s];
                    L.anchors[st.ei - 1].adjust(st.x, st.y);
                }
                let index = L.repeat(points);
                if (index > -1) {
                    let mka = L.anchors,
                        i = index - 1;
                    mka[i].remove(i);
                    points.remove(index);
                    L.plot(points);
                }
            });
        }
    });

    function TextRect(x, y,l) {
         this.x = x;
         this.y = y;
         this.w = 140;
         this.h = 40;
         this.disX = 0;
         this.disY = 0;
         this.L = l;
     }

    TextRect.extend(Shape, {
        draw: function () {
            const n = this,
                text='line',
                dw = n.drawInstance;
            var rect = dw.draw.rect(n.w, n.h).attr({ opacity: 0.1, fill: '#fafafa', x: n.x, y: n.y });
            n.brush = dw.draw.text(text);
            n.brush.attr({
                x: n.x + rect.width() / 2,
                y: n.y + rect.height() / 2
            });
            TextRect.base.Constructor.call(this, rect.id(), text, config.textRect, dw);
            Draw._proto_NC[n.$id] = n;
            return TextRect.base.Parent.prototype.draw.call(this);
        },
        updateContent: function (text) {
            this.name = text;
            this.brush.text(text);
            var size = SVG.get(this.brush.id()).bbox();
            var e=SVG.get(this.$id);
            if (this.w < size.width) {
                e.x(size.x)
                 .size(size.width, this.h);
            }
        },
        move: function (element, d) {
            var self = this;
            self.x = Draw.getClientX(d) - self.disX;
            self.y = Draw.getClientY(d) - self.disY;
            element.attr({
                x: self.x,
                y: self.y
            });
            if (self.brush) {
                self.brush.attr({
                    x: (element.x() + (element.width() / 2)),
                    y: element.y() + (element.height() / 2)
                });
            }
            TextRect.base.Parent.prototype.move.call(this);
        },
        bindEvent: function () {
            this.off('dblclick').on('dblclick', function (evt) {
                evt.preventDefault();
                var n = Draw._proto_NC[this.id()];
                var instance = Draw._proto_LC[n.L];
                instance.exclusionSelect();
                instance.drawInstance.drawOption['dblClick']
                    && instance.drawInstance.
                        drawOption['dblClick'].call(this, instance);
                return false;
            });
        },
        remove: function () {
            var $this = this;
            var n = Draw._proto_NC[$this.$id];
            n.brush.remove();
            SVG.get(n.$id).remove();
            delete Draw._proto_NC[n.$id];
        },
    });

    function Line() {
        this.x1 = 0;
        this.y1 = 0;
        this.x2 = 0;
        this.y2 = 0;
        this.x3 = 0;
        this.y3 = 0;

        this.vs = 89;
        this.ve = 91;
        this.hs = 181;
        this.he = 179;
        this.hzs = 0;
        this.hze = 2;

        this.outline = 10;
        this.order = 0;
        this.border = 2;
        this.expression = '';
        this.url = '';
        this.points = [];
        this.anchors = [];
        this.triangle =null;
        this.coords = [];
    }

    Line.Group = [];

    Line.findById = function (id) {
        for (let i = 0; i < this.Group.length; i++) {
            let group = this.Group[i];
            if (group.id === id) {
                return group.$id;
            }
        }
        return -1;
     }

    Line.extend(Element, {
        constructor: Line,
        draw: function (points) {
            const self = this,
                dw = self.drawInstance;
            const inner = self.drawLine(points, dw.draw.group());
            const L = self.drawLine(points, inner.parent());
            inner.stroke({ width: self.outline }).attr({ opacity: 0 });
            L.stroke({
                width: self.border,
                color: dw.drawOption.backgroundColor
            }).attr({
                "vector-effect": "non-scaling-stroke",
                "shape-rendering": "auto"
            });
            self.coords.push([self.x1, self.y1]);
            self.coords.push([self.x2, self.y2]);
            self.$inner = inner.id();
            Line.Group.push({
                id: inner.id(),
                $id: L.id()
            });
            Line.base.Constructor.call(this, L.id(), "line", "line", dw);
            Draw._proto_LC[self.$id] = this;
            Draw._proto_LC_QC.unshift(this);
            if (!!!points) {
                self.x3 = L.x();
                self.y3 = L.y();
                self.addText();
            }
            self.initEvent.call(inner, self);
            return Line.base.Parent.prototype.draw.call(self);
        },
        drawLine: function (points, l) {
            const self = this;
            const L = (!!points) ? l.polyline(points) : l.polyline([[self.x1, self.y1], [self.x2, self.y2]]);
            L.fill("none");
            return L;
        },
        initEvent: function () {
            this.off('dblclick').on('dblclick', function (evt) {
                evt.preventDefault();
                var instance = Draw._proto_LC[this.id()] ? Draw._proto_LC[this.id()] : Draw._proto_LC[Line.findById(this.id())];
                instance.exclusionSelect();
                if (evt.ctrlKey && evt.altKey) {
                    Draw.removeById(instance.$id);
                    this.off('dblclick');
                    instance.remove();
                }
                else if (evt.ctrlKey && evt.shiftKey) {
                    var mx = Draw.getClientX(evt), my = Draw.getClientY(evt);
                    var points = instance.getPoints();
                    var insertIndex = instance.checkSegment(points, mx, my);
                    if (insertIndex > -1) {
                        points.insert(insertIndex, mx + ',' + my);
                        instance.plot(points);
                        instance.addAnchor();
                    }
                }
                else {
                    if (!Draw._proto_LC_TC[instance.$id]) {
                        var tr = new TextRect(Draw.getClientX(evt), Draw.getClientY(evt));
                        tr.drawInstance = instance.drawInstance;
                        tr.id = Draw.genId(tr.id);
                        tr.draw();
                        Draw._proto_LC_TC[instance.$id] = tr;
                    }
                    instance.drawInstance.drawOption['dblClick']
                        && instance.drawInstance.
                            drawOption['dblClick'].call(this, instance);
                }
                return false;
            });
        },
        bindEvent: function (o) {
            o.initEvent.call(this, o);
            this.parent().on('mouseover', function (evt) {
                evt.preventDefault();
                let children = this.children();
                $.each(children, function () {
                    if (this.type === 'circle') {
                        this.attr({ opacity: 1 });
                    } 
                });
                return false;
            });
            this.parent().on('mouseout', function (evt) {
                evt.preventDefault();
                let children = this.children();
                $.each(children, function () {
                    if (this.type === 'circle') {
                        this.attr({ opacity: 0 });
                    } else if (this.type === 'rect') {
                        this.attr({ visibility: 'hidden' });
                    }
                });
                return false;
            });
        },
        checkSegment: function (points, mx, my) {
            for (var i = 0; i < points.length; i++) {
                var p = points[i];
                var nextIndex = i + 1;
                if (nextIndex < points.length) {
                    let s = this.parseXY(p),
                        a = Draw.angle(s, { x: mx, y: my }),
                        b = Draw.angle(s, this.parseXY(points[nextIndex]));
                    if (a === b) {
                        return nextIndex;
                    }
                }
            }
            return -1;
        },
        checkSegmentTwo: function (mx, my) {
            let points = this.getPoints();
            for (var i = 0; i < points.length; i++) {
                var p = points[i];
                var nextIndex = i + 1;
                if (nextIndex < points.length) {
                    let s = this.parseXY(p),
                        n = this.parseXY(points[nextIndex]),
                        a = Draw.angle(s, { x: mx, y: my }),
                        b = Draw.angle(s, n);
                    if (a === b) {
                        return { x: s.x, y: s.y, x1: n.x, y1: n.y, angle: a, start: i, end: nextIndex };
                    }
                }
            }
            return -1;
        },
        repeat: function (points) {
            for (var i = 0; i < points.length; i++) {
                var p = points[i];
                var nextIndex = i + 1;
                var nextSecordIndex = i + 2;
                if (nextIndex < points.length && nextSecordIndex < points.length) {
                    let s = this.parseXY(p),
                        a = Draw.angle(s, this.parseXY(points[nextIndex])),
                        b = Draw.angle(s, this.parseXY(points[nextSecordIndex]));
                    if (a === b) {
                        return nextIndex;
                    }
                }
            }
            return -1;
        },
        HVSegment: function (points) {
            const self = this;
            let pi = [];
            for (let i = 0; i < points.length; i++) {
                let p = points[i],
                    nextIndex = i + 1;
                if (nextIndex < points.length) {
                    let s = self.parseXY(p),
                        n = self.parseXY(points[nextIndex]),
                        angle = Draw.angle(s, n);
                    if (self.vs <= angle && angle <= self.ve) {
                        pi.push({ type: 'v', start: i, end: nextIndex });
                    } else if ((self.hs <= angle && angle <= self.he) || (self.hzs <= angle && angle <= self.hze)) {
                        pi.push({ type: 'h', start: i, end: nextIndex });
                    }
                }
            }
            return pi;
        },
        autoSegment: function () {
            const self = this;
            let points = self.getPoints();
            let pi = self.HVSegment(points);
            let adjArr = [];
            if (pi.length > 0) {
                let total = points.length - 1;
                for (let i = 0; i < pi.length; i++) {
                    let p = pi[i],
                        start = self.parseXY(points[p.start]),
                        end = self.parseXY(points[p.end]);
                    if (p.type === 'v') {
                        let x = total === p.end ? end.x : start.x;
                        let y = end.y;
                        points[p.end] = x + ',' + y;
                        if (total !== p.end) {
                            adjArr.push({ x: x, y: y, ei: p.end });
                        }
                    } else if (p.type === 'h') {
                        let x = end.x;
                        let y = total === p.end ? end.y : start.y;
                        points[p.end] = x + ',' + y;
                        if (total !== p.end) {
                            adjArr.push({ x: x, y: y, ei: p.end });
                        }
                    }
                }
                self.plot(points);
            }
            return adjArr;
        },
        addText: function () {
            var tr = new TextRect(this.x3, this.y3, this.$id);
            tr.drawInstance = this.drawInstance;
            tr.id = Draw.genId(tr.id);
            tr.draw();
            Draw._proto_LC_TC[this.$id] = tr;
        },
        addArrowTriangle: function () {
            var self = this,
                dw = self.drawInstance,
                L = SVG.get(self.$id);
            L.marker('end', 10, 10, function (add) {
               add.path('M0,0 L0,6 L6,3 z').fill(dw.drawOption.backgroundColor);
               this.attr({
                    refX: 6.7,
                    refY: 3,
                    orient: 'auto',
                    stroke: 'none',
                    markerUNits: 'strokeWidth'
               });
                self.triangle = this;
           });
        },
        addAnchor: function () {
            const self = this;
            const points = self.getPoints();
            let arr = self.anchors;
            if (arr.length > 0) {
                for (let i = 0; i < arr.length; i++) {
                    arr[i].removeElement();
                }
            }
            self.anchors.length = 0;
            points.shift();
            points.pop();
            if (points.length > 0) {
                $.each(points, function () {
                    let p = self.parseXY(this);
                    const anchor = new Anchor(p.x, p.y, self);
                    anchor.drawInstance = self.drawInstance;
                    anchor.draw();
                    self.anchors.push(anchor);
                });
                self.redraw();
            }
        },
        addHV: function (x, y) {
            var self = this;
            if (!self.h) {
                var h = new Horizontal(x, y, self);
                h.drawInstance = self.drawInstance;
                h.draw();
                self.h = h;
            }
            if (!self.v) {
                var v = new Vertical(x, y, self);
                v.drawInstance = self.drawInstance;
                v.draw();
                self.v = v;
            }
        },
        updateContent: function (text) {
            this.name = text;
            if (Draw._proto_LC_TC[this.$id]) {
                Draw._proto_LC_TC[this.$id].updateContent(text);
            } else {
                this.addText();
            }
        },
        exclusionSelect: function () {
            let c= SVG.get(this.$id);
            c.stroke('red');
            this.triangle.stroke('red');
            this.resetArrowTriangle('red');
            for (let id in Draw._proto_LC) {
                if (this.$id!== id) {
                    let L = Draw._proto_LC[id];
                    SVG.get(L.$id).stroke('#000000');
                    L.triangle.stroke('#000000');
                    L.resetArrowTriangle('#000000');
                }
            }
        },
        resetArrowTriangle: function (color) {
            let children = this.triangle.children()
            for (let i = 0; i < children.length; i++) {
                let child = children[i];
                child.fill(color);
            }
        },
        move: function (evt) {
            var self = this,
                dw = self.drawInstance,
                instance = SVG.get(self.$id);
            if (dw.source.to && evt) {
                var nl = Draw._proto_NC[dw.source.to];
                var position = nl.bound(Draw.getClientX(evt), Draw.getClientY(evt));
                if (position) {
                    self.x2 = position.x;
                    self.y2 = position.y;
                }
            }
            self.coords.pop();
            self.coords.push([self.x2, self.y2]);
            instance && instance.plot(self.coords);
        },
        remove: function () {
            var $this = this,
                L = SVG.get($this.$id);
            L.parent().remove();
            $.each($this.anchors, function () {
                delete Draw._proto_NC[this.$id];
            });
            if (Draw._proto_LC_TC[$this.$id]) {
                var tr = Draw._proto_LC_TC[$this.$id];
                tr.remove();
                delete Draw._proto_LC_TC[$this.$id];
            }
            delete Draw._proto_LC[$this.$id];
            $this.anchors.length = 0;
        },
        first: function () {
            var pointArray = this.getPoints();
            var point = pointArray[0];
            var xy = point.split(",");
            return {
                x: parseInt(xy[0]),
                y: parseInt(xy[1])
            };
        },
        parseXY: function (point) {
            var xy = point.split(",");
            return {
                x: parseInt(xy[0]),
                y: parseInt(xy[1])
            };
        },
        last: function () {
            var pointArray = this.getPoints();
            var point = pointArray[pointArray.length - 1];
            var xy = point.split(",");
            return {
                x: parseInt(xy[0]),
                y: parseInt(xy[1])
            };
        },
        last2: function () {
            var pointArray = this.getPoints();
            var point = pointArray.length >= 2 ? pointArray[pointArray.length - 2] : pointArray[0];
            var xy = point.split(",");
            return {
                x: parseInt(xy[0]),
                y: parseInt(xy[1])
            };
        },
        setFirst: function () {
            var pointArray = this.getPoints();
            pointArray.shift();
            pointArray.unshift([this.x1, this.y1].join(','));
            this.plot(pointArray);
        },
        setLast: function () {
            var pointArray = this.getPoints();
            pointArray.pop();
            pointArray.push([this.x2, this.y2].join(','));
            this.plot(pointArray);
        },
        getPoints: function () {
            var self = this,
                L = SVG.get(self.$id),
                points = L.attr("points");

            return points.split(" ");
        },
        redraw: function () {
            var $this = this,
                pointArray = [],
                first = $this.first(),
                last = $this.last();
            pointArray.push([first.x, first.y].join(','));
            $.each($this.anchors, function () {
                pointArray.push([this.x, this.y].join(','));
            });
            pointArray.push([last.x, last.y].join(','));
            $this.plot(pointArray);
        },
        plot: function (pointArray) {
            const L = Draw._proto_LC[this.$id],
                e = SVG.get(L.$id),
                i = SVG.get(L.$inner);
            let s = Array.from(new Set(pointArray));
            i.plot(s.join(" "));
            e.plot(s.join(" "));
        }
    });

    Line.move = function (current) {
        var toElements = Draw.findById(current.$id, "to"),
            fromElements = Draw.findById(current.$id, "from");
        $.each(toElements, function () {
            var el = SVG.get(this.id),
                instance = Draw._proto_LC[this.id];
            if (el && instance) {
                instance.x2 = current.x + this.ox2;
                instance.y2 = current.y + this.oy2;
                instance.setLast();
            }
        });

        $.each(fromElements, function () {
            var el = SVG.get(this.id),
                instance = Draw._proto_LC[this.id];
            if (el && instance) {
                instance.x1 = current.x + this.ox1;
                instance.y1 = current.y + this.oy1;
                instance.setFirst();
            }
        });
    }

    Line.update = function (current) {
        if (current.drawInstance.support) {
            var draw = document.getElementById(current.drawInstance.drawOption.container),
                svg = draw.firstElementChild,
                el = document.getElementById(current.$id);
            svg.removeChild(el);
            var cloneNode = el.cloneNode();
            svg.appendChild(cloneNode);
            var instance = Draw._proto_LC[current.$id];
            instance.bindEvent.call(SVG.get(current.$id), this);
        }
    }

    function Node() {
        this.w = 140;
        this.h = 70;
        this.x = 10;
        this.y = 10;
        this.disX = 0;
        this.disY = 0;
    }

    Node.extend(Shape, {
        draw: function () {
            var n = this,
                dw = n.drawInstance,
                text = '节点';

            var strokeColor = dw.drawOption.strokeColor;
            var rect = dw.draw.rect(n.w, n.h)
                .attr({ fill: '#fafafa', x: n.x, y: n.y, stroke: strokeColor });

            n.brush = dw.draw.text(text);
            n.brush.attr({
                x: n.x + rect.width() / 2,
                y: n.y + rect.height() / 2 
            });
            Node.base.Constructor.call(this, rect.id(), text, "node", dw);
            Draw._proto_NC[n.$id] = n;
            return Node.base.Parent.prototype.draw.call(this);
        },
        updateContent: function (text) {
            this.name = text;
            this.brush.text(text);
        },
        bound: function (moveX, moveY) {
            var x = this.x,
                y = this.y,
                w = this.w,
                h = this.h,
                tickness = this.tickness,
                xt = x + w,
                yt = y + h;

            var direction = {
                bottom: function (moveX, moveY) {
                    var center = {
                        x: x + w / 2,
                        y: yt
                    };
                    return (x + tickness <= moveX
                        && xt - tickness >= moveX
                        && moveY >= yt - tickness
                        && moveY <= yt) ? center : false;
                },
                top: function (moveX, moveY) {

                    var center = {
                        x: x + w / 2,
                        y: y
                    };
                    return (x + tickness <= moveX && xt - tickness >= moveX
                        && moveY >= y
                        && moveY <= y + tickness) ? center : false;

                },
                left: function (moveX, moveY) {
                    var center = {
                        x: x,
                        y: y + h / 2
                    };

                    return (
                        x <= moveX
                        && x + tickness >= moveX
                        && moveY >= y
                        && moveY <= yt
                    ) ? center : false;

                },
                right: function (moveX, moveY) {

                    var center = {
                        x: xt,
                        y: y + h / 2
                    };

                    return (
                        xt - tickness <= moveX
                        && xt >= moveX
                        && moveY >= y
                        && moveY <= yt
                    ) ? center : false;
                }
            }

            for (var propertName in direction) {
                var _check = direction[propertName](moveX, moveY);
                if (_check) {
                    return _check;
                }
            }

            return false;
        },
        move: function (element, d) {
            var self = this;
            self.x = Draw.getClientX(d) - self.disX;
            self.y = Draw.getClientY(d) - self.disY;
            element.attr({
                x: self.x,
                y: self.y
            });
            if (self.brush && this.category === 'node') {
                self.brush.attr({
                    x: (element.x() + (element.width() / 2)),
                    y: element.y() + (element.height() / 2)
                });
            }
            Node.base.Parent.prototype.move.call(this);
        },
        validate: function () {
             return true;
        }
    });

    function Circle(x,y,disX,disY,id,name,category,dw) {
        this.x = x;
        this.y = y;
        this.disX = disX;
        this.disY = disY;
        Circle.base.Constructor.call(this,id, name, category,dw);
    }

    Circle.extend(Shape, {
        draw: function () {
            return Circle.base.Parent.prototype.draw.call(this);
        },
        move: function (element, d) {
            var self = this;
            self.x = Draw.getClientX(d) - self.disX;
            self.y = Draw.getClientY(d) - self.disY;
            element.move(self.x, self.y);
            if (self.brush) {
                self.brush.attr({
                    x: element.x() + element.width() / 2,
                    y: element.y() + element.height() / 2
                });
            }
            Circle.base.Parent.prototype.move.call(self);
        },
        validate: function () {
            return true;
        },
        bound: function (mX, mY) {
            var r = 25,
                c = 5,
                cx = this.x + r,
                cy = this.y,
                z = r * 2;
            var tickness = this.tickness;

            var direction = {
                bottom: {
                    x1: cx - r,
                    y1: cy + r,
                    x2: cx - r,
                    y2: cy + r - tickness,
                    x3: cx + z - r,
                    y3: cy + r - tickness,
                    x4: cx - r + z,
                    y4: cy + r,
                    check: function (moveX, moveY) {
                        var center = {
                            x: cx + c,
                            y: cy + r - c
                        };

                        return this.x1 <= moveX &&
                            this.x3 >= moveX &&
                            this.y1 >= moveY &&
                            this.y2 <= moveY ? center : false;
                    }

                },
                top: {
                    x1: cx - r,
                    y1: cy - r,
                    x2: cx - r,
                    y2: cy - r + tickness,
                    x3: cx + z - r,
                    y3: cy - r,
                    x4: cx + z - r,
                    y4: cy - r + tickness,
                    check: function (moveX, moveY) {

                        var center = {
                            x: cx + c,
                            y: cy - r + c
                        };

                        return this.x1 <= moveX &&
                            this.x3 >= moveX &&
                            this.y1 <= moveY &&
                            this.y2 >= moveY ? center : false;
                    }
                },
                left: {
                    x1: cx - r,
                    y1: cy - r + tickness,
                    x2: cx - r,
                    y2: cy + r - tickness,
                    x3: cx - r + tickness,
                    y3: cy - r + tickness,
                    x4: cx - r + tickness,
                    y4: cy + r - tickness,
                    check: function (moveX, moveY) {

                        var center = {
                            x: cx - r + c * 2,
                            y: cy
                        };

                        return this.x1 <= moveX &&
                            this.x3 >= moveX &&
                            this.y1 <= moveY &&
                            this.y2 >= moveY ? center : false;

                    }
                },
                right: {
                    x1: cx + r,
                    y1: cy - r + tickness,
                    x2: cx + r,
                    y2: cy + r - tickness,
                    x3: cx + r - tickness,
                    y3: cy - r + tickness,
                    x4: cx + r - tickness,
                    y4: cy + r - tickness,
                    check: function (moveX, moveY) {

                        var center = {
                            x: cx + r,
                            y: cy
                        };

                        return this.x1 >= moveX &&
                            this.x3 <= moveX &&
                            this.y1 <= moveY &&
                            this.y2 >= moveY ? center : false;
                    }
                }
            }
            for (var propertName in direction) {
                var _o = direction[propertName],
                    check = _o.check(mX, mY);
                if (check) {
                    return check;
                }
            }
            return false;
        }
    });

    function Decision() {
        this.x = 10;
        this.y = 10;
        this.disX = 0;
        this.disY = 0;
    }

    Decision.extend(Shape, {
        draw: function () {
            const dw = this.drawInstance,color = dw.drawOption.backgroundColor;
            this.drawInstance._decision
                .node
                .firstElementChild
                .instance.attr({ fill: '#fafafa', stroke: color });
            const el = dw.draw.use(this.drawInstance._decision);
            Decision.base.Constructor.call(this, el.id(), '分支节点', 'decision', dw);
            el.move(this.x, this.y);
            Draw._proto_NC[this.$id] = this;
            Decision.base.Parent.prototype.draw.call(this);
        },
        set: function (o) {
            Draw._proto_LC[o.id].expression = o.expression;
        },
        move: function (element, d) {
            var self = this;
            self.x = Draw.getClientX(d) - self.disX;
            self.y = Draw.getClientY(d) - self.disY;
            element.attr({ x: self.x, y: self.y });
            Decision.base.Parent.prototype.move.call(this);
        },
        validate: function () {
            return true;
        },
        bound: function (mX, mY) {

            var n = SVG.get(this.$id);

            var x = n.x(),
                y = n.y();

            var direction = {
                bottom: function (moveX, moveY) {
                    var AX = x + 25;
                    var AY = y + 12.5;

                    var BX = x + 50;
                    var BY = y + 25;

                    var CX = BX + 25;
                    var CY = AY;

                    var center = {
                        x: BX,
                        y: BY
                    };

                    return (AX <= moveX && CX >= moveX
                        && moveY >= AY
                        && moveY <= BY) ? center : false;
                },
                top: function (moveX, moveY) {

                    var AX = x + 25;
                    var AY = y - 12.5;

                    var BX = x + 50;
                    var BY = y - 25;

                    var CX = BX + 25;
                    var CY = AY;

                    var center = {
                        y: BY,
                        x: BX
                    };

                    return (AX <= moveX && CX >= moveX
                        && moveY >= BY
                        && moveY <= AY) ? center : false;
                },
                left: function (moveX, moveY) {
                    var AX = x + 25;
                    var AY = y - 12.5;

                    var BX = x;
                    var BY = y;

                    var CX = BX + 25;
                    var CY = BY + 12.5;

                    var center = {
                        x: BX,
                        y: BY
                    };

                    return BX <= moveX && AX >= moveX
                        && moveY >= AY
                        && moveY <= CY ? center : false;
                },
                right: function (moveX, moveY) {

                    var BX = x + 100;
                    var BY = y;

                    var AX = BX - 25;
                    var AY = BY - 12.5;

                    var CX = BX + 25;
                    var CY = BY + 12.5;

                    var center = {
                        x: BX,
                        y: BY
                    };

                    return (AX <= moveX && BX >= moveX
                        && moveY >= AY
                        && moveY <= CY) ? center : false;
                }
            };

            for (var propertName in direction) {
                var _check = direction[propertName](mX, mY);
                if (_check) {
                    return _check;
                }
            }
            return false;
        }
    });

    function Start() {
    }

    Start.extend(Circle, {
        draw: function () {
            var dw = this.drawInstance;
            /*var path = dw.path("M0,0 a30 30 0 1 0 0 -0.1").
                fill("#eee").
                stroke({
                    width: 1,
                    color: "#ccc"
                });*/
            var strokeColor = dw.drawOption.strokeColor;
            var g = dw.draw.group()
                /*.add(path)*/
                .add(dw.draw.path("M10,0 a20 20 0 1 0 0 -0.1").fill("#fafafa"));
            dw.draw.defs().add(g);
            var start = dw.draw.use(g);
            Start.base.Constructor.call(this, this.x, this.y, this.disX, this.disY, start.id(), "开始", "start", dw);
            start.move(this.x, this.y)
                .attr("stroke", strokeColor);
            Draw._proto_NC[this.$id] = this;
            Start.base.Parent.prototype.draw.call(this);
        },
        bindEvent: function (n) {
            Start.base.Parent.prototype.bindEvent.call(this, n);
            //  this.off('dblclick');
        },
        validate: function () {
            return Draw.findById(this.$id, 'from').length > 0
                && Draw.findById(this.$id, 'to').length === 0;
        }
    });

    function End() {
    }

    End.extend(Circle, {
        constructor: End,
        draw: function () {
            var dw = this.drawInstance/*,
                path = dw.path("M0,0 a30 30 0 1 0 0 -0.1")
                    .stroke({ width: 1, color: "#ccc" })
                    .fill("#eee")*/;
            var strokeColor = dw.drawOption.strokeColor;
            var group = dw.draw.group()
                //.add(path)
                .add(dw.draw.path("M10,0 a20 20 0 1 0 0 -0.1").fill(strokeColor));
            dw.draw.defs().add(group);
            var end = dw.draw.use(group);
            End.base.Constructor.call(this, this.x,this.y, this.disX, this.disY,end.id(), '结束', 'end', dw);
            end.move(this.x, this.y);
            Draw._proto_NC[this.$id] = this;
            End.base.Parent.prototype.draw.call(this);
        },
        
        bindEvent: function (n) {
            End.base.Parent.prototype.bindEvent.call(this, n);
            //this.off('dblclick');
        },
        validate: function () {
            return (Draw.findById(this.$id, 'from').length === 0
                && Draw.findById(this.$id, 'to').length > 0);
        }
    });

    function Join() {
    }

    Join.extend(Circle, {
        draw: function () {
            const dw = this.drawInstance;
            const g = dw.draw.group()
                .add(dw.draw.path("M10,0 a10 10 0 1 0 0 -0.1").fill(dw.drawOption.backgroundColor));
            dw.draw.defs().add(g);
            var n = dw.draw.use(g);
            this.init(n, dw);
            n.move(this.x, this.y);
            Draw._proto_NC[this.$id] = this;
            Join.base.Parent.prototype.draw.call(this);
        },
         init: function (n, dw) {
            Join.base.Constructor.call(this, this.x, this.y, this.disX, this.disY, n.id(), '聚合', 'join', dw);
            this.tickness = 10;
        },
         bindEvent: function (n) {
            Join.base.Parent.prototype.bindEvent.call(this, n);
             //  this.off('dblclick');
         },
         validate: function () {
             return Draw.findById(this.$id, 'from').length > 0
                 && Draw.findById(this.$id, 'to').length > 1;
         },
         move: function (element, d) {
             var self = this;
             self.x = Draw.getClientX(d) - self.disX;
             self.y = Draw.getClientY(d) - self.disY;
             element.move(self.x, self.y);
             if (self.brush) {
                 self.brush.attr({
                     x: element.x() + element.width() / 2,
                     y: element.y() + element.height() / 2
                 });
             }
             Circle.base.Parent.prototype.move.call(self);
         },
         bound: function (mX, mY) {
             var r = 15,
                 c = 5,
                 cx = this.x + r,
                 cy = this.y,
                 z = r * 2;
             var tickness = this.tickness;
             var direction = {
                 bottom: {
                     x1: cx - r,
                     y1: cy + r,
                     x2: cx - r,
                     y2: cy + r - tickness,
                     x3: cx + z - r,
                     y3: cy + r - tickness,
                     x4: cx - r + z,
                     y4: cy + r,
                     check: function (moveX, moveY) {
                         var center = {
                             x: cx + c,
                             y: cy + r - c
                         };

                         return (this.x1 <= moveX &&
                             this.x3 >= moveX &&
                             this.y1 >= moveY &&
                             this.y2 <= moveY) ? center : false;
                     }

                 },
                 top: {
                     x1: cx - r,
                     y1: cy - r,
                     x2: cx - r,
                     y2: cy - r + tickness,
                     x3: cx + z - r,
                     y3: cy - r,
                     x4: cx + z - r,
                     y4: cy - r + tickness,
                     check: function (moveX, moveY) {

                         var center = {
                             x: cx + c,
                             y: cy - r + c
                         };

                         return (this.x1 <= moveX &&
                             this.x3 >= moveX &&
                             this.y1 <= moveY &&
                             this.y2 >= moveY) ? center : false;
                     }
                 },
                 left: {
                     x1: cx - r,
                     y1: cy - r + tickness,
                     x2: cx - r,
                     y2: cy + r - tickness,
                     x3: cx - r + tickness + 10,
                     y3: cy - r + tickness,
                     x4: cx - r + tickness,
                     y4: cy + r - tickness,
                     check: function (moveX, moveY) {

                         var center = {
                             x: cx - r + c * 2,
                             y: cy
                         };

                         return this.x1 <= moveX &&
                             this.x3 >= moveX &&
                             this.y1 <= moveY &&
                             this.y2 >= moveY ? center : false;

                     }
                 },
                 right: {
                     x1: cx + r,
                     y1: cy - r + tickness,
                     x2: cx + r,
                     y2: cy + r - tickness,
                     x3: cx + r - tickness,
                     y3: cy - r + tickness,
                     x4: cx + r - tickness,
                     y4: cy + r - tickness,
                     check: function (moveX, moveY) {

                         var center = {
                             x: cx + r,
                             y: cy
                         };

                         return (this.x1 >= moveX &&
                             this.x3 <= moveX &&
                             this.y1 <= moveY &&
                             this.y2 >= moveY) ? center : false;
                     }
                 }
             }
             for (var propertName in direction) {
                 var _o = direction[propertName],
                     check = _o.check(mX, mY);
                 if (check) {
                     return check;
                 }
             }
             return false;
         }
     });

    function Fork() {
         this.w = 80;
         this.h = 20;
         this.x = 10;
         this.y = 10;
     }

    Fork.extend(Shape, {
         draw: function () {
            var n = this,
                text ='分叉',
                dw = n.drawInstance;
             var strokeColor = dw.drawOption.strokeColor;
             var rect = dw.draw.rect(n.w, n.h)
                .attr({ fill: '#fafafa', x: n.x, y: n.y, stroke: strokeColor });
            n.brush = dw.draw.text(text);
            n.brush.attr({
                x: n.x + rect.width() / 2,
                y: n.y + rect.height() / 2 
            });
            Fork.base.Constructor.call(this, rect.id(), text, 'fork', dw);
            Draw._proto_NC[n.$id] = n;
            return Fork.base.Parent.prototype.draw.call(this);
         },
         bound: function (moveX, moveY) {
             var x = this.x,
                 y = this.y,
                 w = this.w,
                 h = this.h,
                 tickness = this.tickness - 10,
                 xt = x + w,
                 yt = y + h;

             var direction = {
                 bottom: function (moveX, moveY) {
                     var center = {
                         x: x + w / 2,
                         y: yt
                     };
                     return (x + tickness <= moveX
                         && xt - tickness >= moveX
                         && moveY >= yt - tickness
                         && moveY <= yt) ? center : false;
                 },
                 top: function (moveX, moveY) {
                     var center = {
                         x: x + w / 2,
                         y: y
                     };
                     return (x + tickness <= moveX && xt - tickness >= moveX
                         && moveY >= y
                         && moveY <= y + tickness) ? center : false;
                 },
                 left: function (moveX, moveY) {
                     var center = {
                         x: x,
                         y: y + h / 2
                     };

                     return (
                         x <= moveX
                         && x + tickness >= moveX
                         && moveY >= y
                         && moveY <= yt
                     ) ? center : false;

                 },
                 right: function (moveX, moveY) {

                     var center = {
                         x: xt,
                         y: y + h / 2
                     };

                     return (
                         xt - tickness <= moveX
                         && xt >= moveX
                         && moveY >= y
                         && moveY <= yt
                     ) ? center : false;
                 }
             };

             for (var propertName in direction) {
                 var _check = direction[propertName](moveX, moveY);
                 if (_check) {
                     return _check;
                 }
             }

             return false;
         },
         move: function (element, d) {
             var self = this;
             self.x = Draw.getClientX(d) - self.disX;
             self.y = Draw.getClientY(d) - self.disY;
             element.attr({
                 x: self.x,
                 y: self.y
             });

             self.brush.attr({
                x: element.x() + (element.width() / 2),
                 y: element.y() + (element.height() / 2)
             });

             Fork.base.Parent.prototype.move.call(this);
         },
         validate: function () {
             return Draw.findById(this.$id, 'to').length == 1
                 && Draw.findById(this.$id, 'from').length > 1;
         }
     });

    function XML(xml, support) {
        this.xml = xml;
        this.support = support;
        this.docXml = undefined;
        this.root = {};
        this.init();
    }

    XML.style = {
        type: 'object',
        struct: {
            type: 'array',
            id: 'value',
            name: 'value',
            layout: 'value',
            category: 'value',
            rule: 'value',
            group: {
                type: 'array',
                id: 'value',
                name: 'value'
            },
            organization: {
                type: 'array',
                id: 'value',
                name: 'value'
            },
            actor: {
                type: 'array',
                id: 'value',
                name: 'value'
            },
            action: {
                type: 'array',
                id: 'value',
                name: 'value'
            },
            transition: {
                type: 'array',
                destination: 'value',
                layout: 'value',
                id: 'value',
                name: 'value',
                x3: 'value',
                y3:'value',
                direction: 'value',
                order: 'value',
                expression: 'text',
                url: 'value'
            }
        }
    };

    XML.nodeToObject = function (nodeWrap, propertyName, node) {
        for (var i = 0, c = node.childNodes.length; i < c; i++) {
            var n = node.childNodes[i];
            if (n.nodeName == propertyName) {
                nodeWrap[propertyName] = XML.value(n);
            }
        }
    }

    XML.value = function (n) {
        return (!!n.textContent) ? n.textContent : n.text;
    }

    XML.readAttributes = function (nodeWrap, nodeAttribute, node) {
        for (var propertyName in nodeAttribute) {
            if (propertyName === 'type') continue;
            if (typeof nodeAttribute[propertyName] === 'string') {
                var valueType = nodeAttribute[propertyName];
                if (valueType == 'value') {
                    for (var i = 0, len = node.attributes.length; i < len; i++) {
                        var attr = node.attributes[i];
                        if (attr.name === propertyName) {
                            nodeWrap[propertyName] = attr[nodeAttribute[propertyName]];
                            break;
                        }
                    }
                }
                else if (valueType == 'float') {
                    for (var i = 0, len = node.attributes.length; i < len; i++) {
                        var attr = node.attributes[i];
                        if (attr.name === propertyName) {
                            nodeWrap[propertyName] = parseFloat(attr[nodeAttribute[propertyName]]);
                            break;
                        }
                    }
                }
                else {
                    nodeWrap[propertyName] = XML.value(node);
                }

            } else {
                for (var j = 0, count = node.childNodes.length; j < count; j++) {
                    var n = node.childNodes[j];
                    if (n.nodeName == propertyName) {
                        if (nodeAttribute[propertyName].type == 'array' && !nodeWrap[propertyName]) {
                            nodeWrap[propertyName] = [XML.readAttributes(XML.create('object'), nodeAttribute[propertyName], n)];
                        } else if (nodeAttribute[propertyName].type == 'array' && nodeWrap[propertyName]) {
                            nodeWrap[propertyName].push(XML.readAttributes(XML.create('object'), nodeAttribute[propertyName], n));
                        } else if (nodeAttribute[propertyName].type == 'object' && !nodeWrap[propertyName]) {
                            nodeWrap[propertyName] = XML.readAttributes(XML.create('object'), nodeAttribute[propertyName], n);
                        }
                    }
                }
            }
        }
        return nodeWrap;
    }

    XML.create = function (type) {
        return type === 'object' ? {} : [];
    }

    XML.prototype.init = function () {

        if (this.support) {
            this.docXml = new ActiveXObject("Microsoft.XMLDOM");
            this.docXml.async = false;
            this.docXml.loadXML(this.xml);
        } else {
            this.docXml = new DOMParser()
                .parseFromString(this.xml, "text/xml");
        }

        var el = this.docXml.firstChild,
            nodeName = el.nodeName,
            $this = this;

        this.root[nodeName] = XML.create(XML.style.type);

        $.each(el.attributes, function () {
            $this.root[nodeName][this.name] = this[XML.style[this.name]];
        });

        this.parse(this.root[nodeName], el.childNodes);
    }

    XML.prototype.parse = function (workflow, nodes) {
        var nodeArray = [];
        var nodeAttribute = XML.style.struct;
        $.each(nodes, function () {
            nodeArray.push(
                XML.readAttributes({
                    category: this.nodeName
                }, nodeAttribute, this)
            );
        });

        workflow.nodes = nodeArray;
    };

    initialize(function (option) {
        Draw._proto_Cc = new Draw(option);
        return Draw._proto_Cc;
    }, Draw);

})(function (factory,Draw) {
    $.fn.SMF = function (option) {
        return factory(option)
    }
    $.SMF = {
        getInstanceComponent: function () {
            return Draw._proto_Cc;
        },
        getNodeById: function (id) {
            return Draw._proto_NC[id];
        },
        getLineById: function (id) {
            return Draw._proto_LC[id];
        }
    }
})
