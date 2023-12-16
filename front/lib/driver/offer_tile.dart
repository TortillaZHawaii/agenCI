import 'package:flutter/material.dart';

class OfferTile extends StatelessWidget {
  const OfferTile({
    super.key,
    required this.parkingName,
    required this.parkingAddress,
    required this.price,
    required this.onTap,
  });

  final String parkingName;
  final String parkingAddress;
  final double price;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return ListTile(
      title: Text(parkingName),
      subtitle: Text(parkingAddress),
      trailing: Text("${price.toStringAsFixed(2)}€"),
      onTap: onTap,
    );
  }
}
