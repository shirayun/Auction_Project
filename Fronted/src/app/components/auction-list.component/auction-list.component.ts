import { Component, OnInit } from '@angular/core';
import { AuctionService } from '../../services/auction.service';
import { Auction } from '../../Models/auction';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SignalRService } from '../../services/signalr.service';
@Component({
  selector: 'app-auction-list',
  templateUrl: './auction-list.component.html',
  standalone: true,
  imports: [FormsModule,RouterModule, CommonModule]
})
export class AuctionListComponent implements OnInit {

  auctions: Auction[] = [];

  constructor(
  private auctionService: AuctionService,
  private signalRService: SignalRService
) {}

  ngOnInit() {
    this.auctionService.getAuctions().subscribe(data => {
      this.auctions = data;
    });

    this.signalRService.startConnection();

    this.signalRService.onBidUpdate((data) => {
      this.updateAuction(data);
    });
  }

  updateAuction(data: any) {
    const auction = this.auctions.find(a => a.id === data.auctionId);
    if (auction) {
      auction.currentPrice = data.price;
    }
  }
}