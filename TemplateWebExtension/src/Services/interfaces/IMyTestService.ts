import { serviceName } from "@docsvision/web/core/services";
import {IMyTestRequestModel} from "../../Models/IMyTestRequestModel";
import {IMyTestModel} from "../../Models/IMyTestModel";
import { IMyTestMemberRequestModel } from "../../Models/IMyTestMemberRequestModel";
import { IMyTestDateRequsetModel } from "../../Models/IMyTestDateRequestModel";
import { IMyTicketRequestModel } from "../../Models/IMyTicketRequestModel";

export interface IMyTestService{
    GetName(model: IMyTestRequestModel): Promise<IMyTestModel>;
    GetMemberSentData(model: IMyTestMemberRequestModel): Promise<IMyTestModel>;
    ChangeMoneyData(model: IMyTestMemberRequestModel): Promise<IMyTestModel>;
    ChangeDayCount(model: IMyTestDateRequsetModel): Promise<IMyTestModel>;
    GetTripData(model: IMyTestRequestModel): Promise<IMyTestModel>;
    GetTestData(model: IMyTestRequestModel): Promise<IMyTestModel>;
    GetTicketsData(model: IMyTicketRequestModel): Promise<IMyTestModel>;
}

export type $MyTestService = { activityPlanService: IMyTestService };
export const $MyTestService = serviceName<$MyTestService, IMyTestService>(x => x.activityPlanService)