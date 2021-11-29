import { Pipe, PipeTransform } from "@angular/core";
import { Round } from "../models/round";
@Pipe({name: 'RoundConvert'})
export class RoundConverter implements PipeTransform {
    transform(value: Round):string {
        return Round[value];
    }

}
