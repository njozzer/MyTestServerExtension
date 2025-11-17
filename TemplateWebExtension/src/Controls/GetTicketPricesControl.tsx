import { BaseControl, BaseControlParams, BaseControlState } from "@docsvision/webclient/System/BaseControl";
import { ControlImpl } from "@docsvision/webclient/System/ControlImpl";
import { r } from "@docsvision/webclient/System/Readonly";
import React from "react";
import { $MyTestService } from "../Services/interfaces/IMyTestService";
import { $CardId } from "@docsvision/webclient/System/LayoutServices";
import { Button, ButtonAlignModes } from "@docsvision/webclient/Helpers/Button";
import { LayoutManager } from "@docsvision/webclient/System/LayoutManager";
import { DirectoryDesignerRow } from "@docsvision/webclient/BackOffice/DirectoryDesignerRow";
import { DateTimePicker } from "@docsvision/webclient/Platform/DateTimePicker";

const localization = ["Запросить стоимость билетов","Выбери билет:"];
export class SuperControlParams extends BaseControlParams {
    @r ticketsPriceLabel?: string;

    @r services?: $MyTestService & $CardId & LayoutManager;
}
export interface SuperControlState extends SuperControlParams,
    BaseControlState {
    res: Array<any>;
    price: number;
}
export class GetTicketPricesControl extends BaseControl<SuperControlParams, SuperControlState> {

    myCollapsableStyle: any;
    construct() {
        super.construct();

        this.state.res = null;
        this.myCollapsableStyle = {
            height: 0,
            overflow: "hidden",
            visibility: "hidden",
            transition: "height 0.3s ease - out, visibility 0.3s ease - out"
        };
    }

    protected createParams(): SuperControlParams {
        return new SuperControlParams();
    }
    protected createImpl() {
        return new ControlImpl(this.props, this.state, this.renderControl.bind(this));
    }
    private async onClick() {
        var layout = this.state.parent.layout;
        
        
        let dateFrom: DateTimePicker = layout.controls.get<DateTimePicker>("dateFrom");
        let dateTo: DateTimePicker = layout.controls.get<DateTimePicker>("dateTo");
        let cityRef: DirectoryDesignerRow = layout.controls.get<DirectoryDesignerRow>("cityRef");
        var dateFrom_ = dateFrom.params.value.getFullYear() + "-" + (dateFrom.params.value.getMonth() + 1) + "-" + dateFrom.params.value.getDate() 
        var dateTo_ = dateTo.params.value.getFullYear() + "-" + (dateTo.params.value.getMonth() + 1) + "-" + dateTo.params.value.getDate()

        var res = await this.state.services.activityPlanService.GetTicketsData({
            documentId: layout.cardInfo.id,
            dateFrom: dateFrom_,
            dateTo: dateTo_,
            cityRef: cityRef.params.value.id
        });
        var data = JSON.parse(res.content);
       
        this.setState({ res: data["data"] });
    }
    private async onRowClick(event) {
        console.log(event.target.value);
        this.setState({ price: event.target.value });

    }
    private async onClickCollapse() {
        var collapsable_ = document.querySelector(".Collapsable_Tickets");
        collapsable_.classList.toggle('show');

        
    }
    renderTickets() {
        var selectId = "select_ticket_id";
        var list = this.state.res.map((item) => {
            return (
                <option id={item.flight_number} value={item.price} >
                    Аэропорт: {item.destination_airport} {", " }
                    Авиакомпания: {item.airline}{", "}
                    Рейс: {item.flight_number}{", "}
                    Цена: {item.price} 
                </option>)
        })
        
        var render = (
            <div style={{ display: "block;" }}>
                <label htmlFor={selectId} style={{ display: "inline-block;", maxWidth:"100%" }} >{localization[1]}</label>
                <select id={selectId} onChange={(event) => this.onRowClick(event)} style={{ display: "inline-block;", maxWidth: "100%" }}>
                    
                    <option value="0" id="select_zero_option">
                        Выбор билета
                    </option>
                    {list}
                </select>
            </div>);
        return render
    }
    renderControl() {
        return (

            <div className="GetTicketControl" >
                
                <div >
                    <Button text={localization[0]} onClick={() => this.onClick()} align={ButtonAlignModes.Center} />

                    {this.state.res != null && (
                        <div >
                            <div className="GTC_div_with_paddings">
                                <span onClick={() => this.onClickCollapse()} style={{ height:30 }}><p>Билеты</p></span>
                            </div>
                            <div className="Collapsable_Tickets" >
                                {this.renderTickets()}

                                {this.state.price != null && (
                                    <div style={{ float:"right" }}>
                                        <p>Цена билета :{this.state.price}   </p>

                                    </div>
                                )}
                            </div>
                            
                        </div>
                    )}
                    
                </div>
            </div>
        );
    }
}
