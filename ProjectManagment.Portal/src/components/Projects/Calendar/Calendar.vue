<template>
  <div>
    <FullCalendar aspectRatio="0.5" defaultView="timeGridWeek" :plugins="calendarPlugins" />
  </div>
</template>

<script>
import FullCalendar from "@fullcalendar/vue";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";

export default {
  name: "ProjectCalendar",
  components: {
    FullCalendar // make the <FullCalendar> tag available
  },
  data() {
    return {
      calendarPlugins: [dayGridPlugin, timeGridPlugin],
      events: []
    };
  },
  methods: {
    AddEvent() {}
  }
};
</script>

<style lang='scss' scoped>
@import "~@fullcalendar/core/main.css";
@import "~@fullcalendar/daygrid/main.css";
@import "~@fullcalendar/timegrid/main.css";
</style>