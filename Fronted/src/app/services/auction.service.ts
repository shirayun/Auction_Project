import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Auction } from '../Models/auction';
import { Bid } from '../Models/bid';

@Injectable({
  providedIn: 'root'
})
export class AuctionService {

  constructor(private http: HttpClient) {}

  getAuctions() {
    return this.http.get<Auction[]>('https://localhost:5001/api/auction');
  }

  getAuction(id: number) {
    return this.http.get<Auction>(`${'https://localhost:5001/api/auction'}/${id}`);
  }

  placeBid(id: number, bid: Bid) {
    return this.http.post(`${'https://localhost:5001/api/auction'}/${id}/bid`, bid);
  }
}