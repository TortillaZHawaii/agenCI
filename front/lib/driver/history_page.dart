import 'package:flutter/material.dart';

class HistoryPage extends StatelessWidget {
  const HistoryPage({
    super.key,
    required this.driverId,
  });

  final String driverId;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Driver $driverId History"),
      ),
      body: const Placeholder(),
    );
  }
}
