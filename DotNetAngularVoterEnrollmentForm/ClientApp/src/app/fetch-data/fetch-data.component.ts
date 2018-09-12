import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent
{
  public states: GetStates[];
  public cities: GetCity[];
  public constituencies: GetConstituency[];
  public wards: GetWard[];
  baseUrl: string;
  searchName: string = "";
  fullName: string = "";
  stateId: number = 0;
  cityId: number = 0;
  consId: number = 0;
  wardId: number = 0;
  fatherName: string = "";
  dob: string = "";
  email: string = "";
  phone: string = "";

 

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<GetStates[]>(baseUrl + 'api/SampleData/GetStates').subscribe(result => {
      this.states = result;
      console.log(result);
    }, error => console.error(error));
  }
  
  GetCity(stateId) {
    alert(stateId);
    var data = stateId;
    this.http.get<GetCity[]>('api/SampleData/GetCity?StateId='+ data).subscribe(result => {
      this.cities = result;
      console.log(result);
    }, error => console.error(error));
   
  }

  GetConstituency(cityId) {
    alert(cityId);
    var data = cityId;
    this.http.get<GetConstituency[]>('api/SampleData/GetConstituency?cityId=' + data).subscribe(result => {
      this.constituencies = result;
      console.log(result);
    }, error => console.error(error));

  }
  GetWardNoDetails(wardNo) {

  }

  GetWardNo(constituencyId) {
    alert(constituencyId);
    var data = constituencyId;
    this.http.get<GetWard[]>('api/SampleData/getWard?constituencyId=' + data).subscribe(result => {
      this.wards = result;
      console.log(result);
    }, error => console.error(error));

  }

  SubmitMe() {
   
    console.log(this.stateId);
    console.log(this.fullName);
    console.log(this.consId);
    console.log(this.fatherName);
    console.log(this.dob);
    console.log(this.cityId);
    console.log(this.wardId);
    console.log(this.phone);
    console.log(this.email);
    var data = new Object();
    data["StateId"] = this.stateId;

    this.http.get<any>('api/SampleData/GetEnrollmentNumber?StateId=' + this.stateId + '&CityId=' + this.cityId + '&ConstituencyId=' + this.consId + '&WardNumberId=' + this.wardId + '&EnrollerName=' + this.fullName + '&Email=' + this.email + '&DOB=' + this.dob + '&PhoneNumber=' + this.phone + '&FatherName=' + this.fatherName).subscribe(result => {
      //this.cities = result;
      console.log(result);
      alert("Data saved successfully!! Please check your email for enrollment number.");
    }, error => console.error(error));
  }
}




interface GetStates {
  StateId: number;
  StateName: string;
  
}
interface GetCity {
  CityId: number;
  CityName: string;

}

interface GetConstituency {
  ConstituencyId: number;
  ConstituencyName: string;

}
interface GetWard {
  WardNumberId: number;
  WardNumber: string;

}

