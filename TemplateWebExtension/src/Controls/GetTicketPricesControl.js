var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
import { BaseControl, BaseControlParams } from "@docsvision/webclient/System/BaseControl";
import { ControlImpl } from "@docsvision/webclient/System/ControlImpl";
import { r } from "@docsvision/webclient/System/Readonly";
import React from "react";
import { Button, ButtonAlignModes } from "@docsvision/webclient/Helpers/Button";
var localization = ["Запросить стоимость билетов", "Выбери билет:"];
var SuperControlParams = /** @class */ (function (_super) {
    __extends(SuperControlParams, _super);
    function SuperControlParams() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        r
    ], SuperControlParams.prototype, "ticketsPriceLabel", void 0);
    __decorate([
        r
    ], SuperControlParams.prototype, "services", void 0);
    return SuperControlParams;
}(BaseControlParams));
export { SuperControlParams };
var GetTicketPricesControl = /** @class */ (function (_super) {
    __extends(GetTicketPricesControl, _super);
    function GetTicketPricesControl() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    GetTicketPricesControl.prototype.construct = function () {
        _super.prototype.construct.call(this);
        this.state.res = null;
        this.myCollapsableStyle = {
            height: 0,
            overflow: "hidden",
            visibility: "hidden",
            transition: "height 0.3s ease - out, visibility 0.3s ease - out"
        };
    };
    GetTicketPricesControl.prototype.createParams = function () {
        return new SuperControlParams();
    };
    GetTicketPricesControl.prototype.createImpl = function () {
        return new ControlImpl(this.props, this.state, this.renderControl.bind(this));
    };
    GetTicketPricesControl.prototype.onClick = function () {
        return __awaiter(this, void 0, void 0, function () {
            var layout, dateFrom, dateTo, cityRef, dateFrom_, dateTo_, res, data;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        layout = this.state.parent.layout;
                        dateFrom = layout.controls.get("dateFrom");
                        dateTo = layout.controls.get("dateTo");
                        cityRef = layout.controls.get("cityRef");
                        dateFrom_ = dateFrom.params.value.getFullYear() + "-" + (dateFrom.params.value.getMonth() + 1) + "-" + dateFrom.params.value.getDate();
                        dateTo_ = dateTo.params.value.getFullYear() + "-" + (dateTo.params.value.getMonth() + 1) + "-" + dateTo.params.value.getDate();
                        return [4 /*yield*/, this.state.services.activityPlanService.GetTicketsData({
                                documentId: layout.cardInfo.id,
                                dateFrom: dateFrom_,
                                dateTo: dateTo_,
                                cityRef: cityRef.params.value.id
                            })];
                    case 1:
                        res = _a.sent();
                        data = JSON.parse(res.content);
                        this.setState({ res: data["data"] });
                        return [2 /*return*/];
                }
            });
        });
    };
    GetTicketPricesControl.prototype.onRowClick = function (event) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                console.log(event.target.value);
                this.setState({ price: event.target.value });
                return [2 /*return*/];
            });
        });
    };
    GetTicketPricesControl.prototype.onClickCollapse = function () {
        return __awaiter(this, void 0, void 0, function () {
            var collapsable_;
            return __generator(this, function (_a) {
                collapsable_ = document.querySelector(".Collapsable_Tickets");
                collapsable_.classList.toggle('show');
                return [2 /*return*/];
            });
        });
    };
    GetTicketPricesControl.prototype.renderTickets = function () {
        var _this = this;
        var selectId = "select_ticket_id";
        var list = this.state.res.map(function (item) {
            return (React.createElement("option", { id: item.flight_number, value: item.price },
                "\u0410\u044D\u0440\u043E\u043F\u043E\u0440\u0442: ",
                item.destination_airport,
                " ",
                ", ",
                "\u0410\u0432\u0438\u0430\u043A\u043E\u043C\u043F\u0430\u043D\u0438\u044F: ",
                item.airline,
                ", ",
                "\u0420\u0435\u0439\u0441: ",
                item.flight_number,
                ", ",
                "\u0426\u0435\u043D\u0430: ",
                item.price));
        });
        var render = (React.createElement("div", { style: { display: "block;" } },
            React.createElement("label", { htmlFor: selectId, style: { display: "inline-block;", maxWidth: "100%" } }, localization[1]),
            React.createElement("select", { id: selectId, onChange: function (event) { return _this.onRowClick(event); }, style: { display: "inline-block;", maxWidth: "100%" } },
                React.createElement("option", { value: "0", id: "select_zero_option" }, "\u0412\u044B\u0431\u043E\u0440 \u0431\u0438\u043B\u0435\u0442\u0430"),
                list)));
        return render;
    };
    GetTicketPricesControl.prototype.renderControl = function () {
        var _this = this;
        return (React.createElement("div", { className: "GetTicketControl" },
            React.createElement("div", null,
                React.createElement(Button, { text: localization[0], onClick: function () { return _this.onClick(); }, align: ButtonAlignModes.Center }),
                this.state.res != null && (React.createElement("div", null,
                    React.createElement("div", { className: "GTC_div_with_paddings" },
                        React.createElement("span", { onClick: function () { return _this.onClickCollapse(); }, style: { height: 30 } },
                            React.createElement("p", null, "\u0411\u0438\u043B\u0435\u0442\u044B"))),
                    React.createElement("div", { className: "Collapsable_Tickets" },
                        this.renderTickets(),
                        this.state.price != null && (React.createElement("div", { style: { float: "right" } },
                            React.createElement("p", null,
                                "\u0426\u0435\u043D\u0430 \u0431\u0438\u043B\u0435\u0442\u0430 :",
                                this.state.price,
                                "   ")))))))));
    };
    return GetTicketPricesControl;
}(BaseControl));
export { GetTicketPricesControl };
//# sourceMappingURL=GetTicketPricesControl.js.map